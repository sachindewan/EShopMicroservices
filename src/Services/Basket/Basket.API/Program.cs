
using Discount.Grpc.Protos;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Correlation;

var builder = WebApplication.CreateBuilder(args);
//Application Services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
//Data Services
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<ICorrelationIdGenerator , CorrelationIdGenerator>();
builder.Services.AddScoped<CorrelationIdMiddleware>();
//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);
var app = builder.Build();
app.UseExceptionHandler(options => { });
app.UseMiddleware<CorrelationIdMiddleware>();
app.MapCarter();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


app.Run();
