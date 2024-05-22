namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<ErrorOr<DeleteProductResult>>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    public class DeleteProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<DeleteProductCommand, ErrorOr<DeleteProductResult>>
    {
        public async Task<ErrorOr<DeleteProductResult>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await documentSession.LoadAsync<Product>(request.Id, cancellationToken);
            if (product == null)
            {
                return ProductErrors.BadRequest;
            }
            documentSession.Delete(product);
            await documentSession.SaveChangesAsync();
            return new DeleteProductResult(true);
        }
    }
}
