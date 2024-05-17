using BuildingBlock.CQRS;
using BuildingBlock.Errors;
using Catalog.API.Models;
using ErrorOr;
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<ErrorOr<CreateProductResult>>;
    public record CreateProductResult(Guid Id);
    public class CreateProductHandler : ICommandHandler<CreateProductCommand, ErrorOr<CreateProductResult>>
    {
        public async Task<ErrorOr<CreateProductResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //create product entity from command object
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };
            //[TODO]
            //save to database
            if(product.Id == Guid.Empty)
            {
                return ProductErrors.CreateFailed;
            }
            //return result
            return new CreateProductResult(product.Id);
        }
    }
}
