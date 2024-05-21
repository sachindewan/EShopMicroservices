﻿namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<ErrorOr<UpdateProductResult>>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").Length(2,150).WithMessage("Name must be between 2 and 150 charecters");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }   
    internal class UpdateProductCommandHandler(IDocumentSession documentSession , ILogger<UpdateProductCommandHandler> logger): ICommandHandler<UpdateProductCommand, ErrorOr<UpdateProductResult>>
    {
        public async Task<ErrorOr<UpdateProductResult>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);
            var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null)
            {
                return ProductErrors.BadRequest;
            }
            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            documentSession.Update(product);
            await documentSession.SaveChangesAsync();
            return new UpdateProductResult(true);
        }
    }
}