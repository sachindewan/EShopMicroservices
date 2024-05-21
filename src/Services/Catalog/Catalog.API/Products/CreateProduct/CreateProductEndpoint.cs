namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send<ErrorOr<CreateProductResult>>(command);
                return result.Match((result) =>
                {
                    var response = result.Adapt<CreateProductResult>();
                    return Results.Created($"/products/{response.Id}", response);
                }, errors =>
                {
                    Error error = errors.FirstOrDefault();
                    return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status400BadRequest);
                });
         
                //var response = result.Adapt<CreateProductResult>();
                //return Results.Created($"/products/{response.Id}", response);
            })
             .WithName("CreateProduct")
             .Produces<CreateProductResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Create Product")
             .WithDescription("Create Product");
        }
    }
}
