using Basket.API.Dtos;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDto basketcheckoutDto);
    public record CheckoutBasketResponse(bool IsSuccess);
    public class CheckoutBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            {
                TypeAdapterConfig<CheckoutBasketRequest, CheckOutBasketCommand>
                         .NewConfig()
                           .Map(dest => dest.basketcheckoutDto, src => src.basketcheckoutDto);
                var command = request.Adapt<CheckOutBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CheckoutBasketResponse>();

                return Results.Ok(response);
            }).WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");
        }
    }
}
