namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send<ErrorOr<UpdateProductResult>>(command);
                return result.Match((result) =>
                {
                    var response = result.Adapt<UpdateProductResponse>();
                    return Results.Ok(response);
                }, errors =>
                {
                    Error error = errors.FirstOrDefault();
                    return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status400BadRequest);
                });
            })
             .WithName("UpdateProduct")
             .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Update Product")
             .WithDescription("Update Product");
        }
    }
}
