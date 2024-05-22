
namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid id) : IQuery<ErrorOr<GetProductByIdResult>>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByIdQuery, ErrorOr<GetProductByIdResult>>
    {
        public async Task<ErrorOr<GetProductByIdResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await documentSession.LoadAsync<Product>(request.id,cancellationToken);
            if (product == null)
            {
                return ProductErrors.NotFound;
            }
            return new GetProductByIdResult(product);
        }
    }
}
