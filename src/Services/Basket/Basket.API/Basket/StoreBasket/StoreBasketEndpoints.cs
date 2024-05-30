using ErrorOr;
using JasperFx.Core;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);
    public class StoreBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender, HttpContext context) =>
            {
                var command = request.Adapt<StoreBasketCommand>();

                return await sender.Send(command).Match(result =>
                {
                    var response = result.Adapt<StoreBasketResponse>();
                    return Results.Created($"/basket/{response.UserName}", response);
                }, Errors =>
                {
                    var error = Errors.FirstOrDefault();
                    return Results.Problem(error.Description, instance: context.Request.Path, title: error.Code, statusCode: StatusCodes.Status400BadRequest);
                });

            })
            .WithName("CreateProduct")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
