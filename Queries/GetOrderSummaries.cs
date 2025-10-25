using MediatR;

public record GetOrderSummariesQuery() : IRequest<IQueryable<OrderSummaryDto>>;