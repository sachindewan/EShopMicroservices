using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send<ErrorOr<GetProductByIdResult>>(query);
                return result.Match(
                                      product =>
                                      {
                                          var response = product.Adapt<GetProductByIdResponse>();
                                          return Results.Ok(response);
                                      },
                                      errors =>
                                      {
                                          var error = errors.FirstOrDefault();
                                          return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status404NotFound);
                                      }
                                                                         );
            })
             .WithName("GetProductById")
             .Produces<CreateProductResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Product by Id")
             .WithDescription("Get Product by Id");
        }
    }
}
