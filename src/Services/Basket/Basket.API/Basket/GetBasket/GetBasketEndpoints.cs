
using ErrorOr;

namespace Basket.API.Basket.GetBasket
{
    //public record GetBasketRequest(string UserName); 
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender,HttpContext context ) =>
            {
                return await sender.Send(new GetBasketQuery(userName)).Match(result =>
                {
                    var respose = result.Adapt<GetBasketResponse>();
                    return Results.Ok(respose);
                }, Errors =>
                {
                    var error = Errors.FirstOrDefault();
                    return Results.Problem(error.Description,instance: context.Request.Path,title:error.Code,statusCode:StatusCodes.Status400BadRequest);
                });
            })
          .WithName("GetProductById")
          .Produces<GetBasketResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get Product By Id")
          .WithDescription("Get Product By Id");
        }
    }
}
