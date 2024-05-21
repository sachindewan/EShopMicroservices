
namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send<ErrorOr<DeleteProductResult>>(command);
                return result.Match((result) =>
                {
                    var response = result.Adapt<DeleteProductResponse>();
                    return Results.Ok(response);
                }, errors =>
                {
                    Error error = errors.FirstOrDefault();
                    return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status400BadRequest);
                });
            })
             .WithName("DeleteProduct")
             .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Delete Product")
             .WithDescription("Delete Product");
        }
    }
}
