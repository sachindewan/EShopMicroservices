namespace Ordering.Application.Order.Queries.GetOrder
{
    public record GetOrdersQuery(PaginationRequest PaginationRequest)
     : IQuery<GetOrdersResult>;

    public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
}
