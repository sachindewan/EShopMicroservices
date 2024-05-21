namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest(string category);
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var result = await sender.Send<ErrorOr<GetProductByCategoryResult>>(query);
                return result.Match(
                                     products =>
                                     {
                                         var response = products.Adapt<GetProductByCategoryResult>();
                                         return Results.Ok(response);
                                     },
                                     errors =>
                                     {
                                         var error = errors.FirstOrDefault();
                                         return Results.Problem(error.Description, title: error.Code, statusCode: StatusCodes.Status404NotFound);
                                     }
                                                                                                                                                                                               );
            })
             .WithName("GetProductByCategory")
             .Produces<GetProductByCategoryResult>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Product by Category")
             .WithDescription("Get Product by Category");
        }
    }
}
