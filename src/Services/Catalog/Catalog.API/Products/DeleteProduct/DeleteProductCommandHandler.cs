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
    public class DeleteProductCommandHandler(IDocumentSession documentSession , ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, ErrorOr<DeleteProductResult>>
    {
        public async Task<ErrorOr<DeleteProductResult>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", request);
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
