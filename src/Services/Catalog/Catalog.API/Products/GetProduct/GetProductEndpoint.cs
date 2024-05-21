
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductQuery());
                return result.Match(
                                       products =>
                                       {
                                           var response = products.Adapt<GetProductResponse>();
                                           return Results.Ok(response);
                                       },
                                                          errors =>
                                                          {
                                                              var error = errors.FirstOrDefault();
                                                              return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status404NotFound);
                                                          }
                                                                             );
            })
             .WithName("GetProduct")
             .Produces<CreateProductResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Products")
             .WithDescription("Get Products"); ;
        }
    }
}
