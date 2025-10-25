using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetOrderSummariesQueryHandler : IRequestHandler<GetOrderSummariesQuery, IQueryable<OrderSummaryDto>>
{
    public readonly ReadDbContext _dbContext;

    public GetOrderSummariesQueryHandler(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<OrderSummaryDto>> Handle(GetOrderSummariesQuery request, CancellationToken cancellationToken)
    {
        var orders = _dbContext.Orders
        .AsNoTracking()
            .OrderBy(o => o.Id)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.FirstName + " " + o.LastName,
                o.Status,
                o.TotalCost
            ));
            
        return await Task.FromResult(orders);
    }
}