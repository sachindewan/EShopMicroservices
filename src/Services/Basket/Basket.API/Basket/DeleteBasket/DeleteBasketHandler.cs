using Basket.API.Errors;
using BuildingBlocks.Errors;
using ErrorOr;
using Marten;
using MediatR;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<ErrorOr<DeleteBasketResult>>;
    public record DeleteBasketResult(bool IsSuccess);
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class DeleteBasketCommandHandler(IBasketRepository repository)
     : ICommandHandler<DeleteBasketCommand, ErrorOr<DeleteBasketResult>>
    {
        public async Task<ErrorOr<DeleteBasketResult>> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            
            var product = await repository.GetBasket(command.UserName, cancellationToken);
            if (product == null)
            {
                return BasketError.NotFoundError(command.UserName);
            }
            await repository.DeleteBasket(command.UserName, cancellationToken);

            return new DeleteBasketResult(true);
        }
    }
}
