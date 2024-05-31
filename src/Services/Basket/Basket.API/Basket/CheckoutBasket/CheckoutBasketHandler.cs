using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckOutBasketCommand(BasketCheckoutDto basketcheckoutDto) : ICommand<CheckOutBasketResult>;
    public record CheckOutBasketResult(bool IsSuccess);
    public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckOutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.basketcheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.basketcheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class CheckoutBasketCommandHandler(IBasketRepository basketRepository , IPublishEndpoint publishEndpoint) : ICommandHandler<CheckOutBasketCommand, CheckOutBasketResult>
    {
        public async Task<CheckOutBasketResult> Handle(CheckOutBasketCommand request, CancellationToken cancellationToken)
        {
            // get existing basket with total price
            // Set totalprice on basketcheckout event message
            // send basket checkout event to rabbitmq using masstransit
            // delete the basket
            var basket = await basketRepository.GetBasket(request.basketcheckoutDto.UserName, cancellationToken);
            if (basket == null)
            {
                return new CheckOutBasketResult(false);
            }
            var eventMessage = request.basketcheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            await publishEndpoint.Publish(eventMessage, cancellationToken);
            await basketRepository.DeleteBasket(request.basketcheckoutDto.UserName, cancellationToken);

            return new CheckOutBasketResult(true);
        }
    }
}
