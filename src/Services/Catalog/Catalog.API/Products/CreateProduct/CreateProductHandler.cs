namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<ErrorOr<CreateProductResult>>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class CreateProductHandler(IDocumentSession documentSession,ILogger<CreateProductHandler> logger) : ICommandHandler<CreateProductCommand, ErrorOr<CreateProductResult>>
    {
        public async Task<ErrorOr<CreateProductResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateProductHandler.Handle called with {@Command}", request);
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };
            documentSession.Store(product);
            await documentSession.SaveChangesAsync();
            if (product.Id == Guid.Empty)
            {
                return ProductErrors.CreateFailed;
            }
            return new CreateProductResult(product.Id);
        }
    }
}
