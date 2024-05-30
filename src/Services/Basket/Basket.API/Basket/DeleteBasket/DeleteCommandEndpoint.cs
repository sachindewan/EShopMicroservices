using ErrorOr;

namespace Basket.API.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender,HttpContext context) =>
            {
               await sender.Send(new DeleteBasketCommand(userName)).Match(result =>
               {
                   var response = result.Adapt<DeleteBasketResponse>();
                   return Results.Ok(response);
               }, Errors =>
               {
                   var error = Errors.FirstOrDefault();
                   return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status400BadRequest,instance: context.Request.Path);
               });
            })
            .WithName("DeleteProduct")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
        }
    }
}
