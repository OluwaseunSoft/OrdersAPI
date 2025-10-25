using MediatR;

public class OrderCreatedProjectionHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly ReadDbContext _readDbContext;

    public OrderCreatedProjectionHandler(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }


    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = notification.OrderId,
            FirstName = notification.FirstName,
            LastName = notification.LastName,
            Status = "Created",
            CreatedAt = DateTime.Now,
            TotalCost = notification.TotalCost
        };

        await _readDbContext.Orders.AddAsync(order, cancellationToken);
        await _readDbContext.SaveChangesAsync(cancellationToken);
    }
}