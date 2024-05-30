
using Basket.API.Errors;
using ErrorOr;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<ErrorOr<GetBasketResult>>;
    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, ErrorOr<GetBasketResult>>
    {
        public async Task<ErrorOr<GetBasketResult>> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(query.UserName);
            if (basket == null)
            {
                return BasketError.NotFoundError(query.UserName);
            }
            return new GetBasketResult(basket);
        }
    }
}
