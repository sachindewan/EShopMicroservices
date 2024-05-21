
namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<ErrorOr<GetProductByCategoryResult>>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession documentSession , ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, ErrorOr<GetProductByCategoryResult>>
    {
        public async Task<ErrorOr<GetProductByCategoryResult>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}",request);
            var products = await documentSession.Query<Product>().Where(p => p.Category.Contains(request.category)).ToListAsync(cancellationToken);
            if (products == null)
            {
                return ProductErrors.NotFound;
            }
            return new GetProductByCategoryResult(products);
        }
    }
}
