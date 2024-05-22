
using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductQuery(int PageNumber = 0, int PageSize = 10) : IQuery<ErrorOr<GetProductResult>>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductQuery, ErrorOr<GetProductResult>>
    {
        public async Task<ErrorOr<GetProductResult>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = await documentSession.Query<Product>().ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
            if (products == null)
            {
                return ProductErrors.NotFound;
            }
            return new GetProductResult(products);
        }
    }
}
