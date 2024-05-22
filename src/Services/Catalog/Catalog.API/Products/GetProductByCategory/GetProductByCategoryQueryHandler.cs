
namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<ErrorOr<GetProductByCategoryResult>>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, ErrorOr<GetProductByCategoryResult>>
    {
        public async Task<ErrorOr<GetProductByCategoryResult>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await documentSession.Query<Product>().Where(p => p.Category.Contains(request.category)).ToListAsync(cancellationToken);
            if (products == null)
            {
                return ProductErrors.NotFound;
            }
            return new GetProductByCategoryResult(products);
        }
    }
}
