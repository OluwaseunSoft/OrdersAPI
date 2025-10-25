using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection")));
builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("WriteDbConnection")));
builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ReadDbConnection")));

//builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
//builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
//builder.Services.AddScoped<IQueryHandler<GetOrderSummariesQuery, IQueryable<OrderSummaryDto>>, GetOrderSummariesQueryHandler>();
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
//builder.Services.AddScoped<IEventPublisher, InProcessEventPublisher>();
//builder.Services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedProjectionHandler>();

builder.Services.AddMediatR(cfg=> cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapPost("/api/orders", async (IMediator mediator, CreateOrderCommand command) =>
{
    try
    {
    var createdOrder = await mediator.Send(command);
    if(createdOrder is null) return Results.BadRequest("Failed to create order");
        return Results.Created($"/api/orders/{createdOrder.Id}", createdOrder);
    }
    catch(ValidationException ex)
    {
        return Results.BadRequest(ex.Errors.Select(e=> new { e.PropertyName, e.ErrorMessage }));
    }
});

app.MapGet("/api/orders/{id}", async (IMediator mediator, int id) =>
{
    //var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
    var order = await mediator.Send(new GetOrderByIdQuery(id));
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapGet("/api/orders", async (IMediator mediator, [AsParameters] Pagination page) =>
{
    var orderSummaries = await mediator.Send(new GetOrderSummariesQuery());
    if (orderSummaries is null) return Results.NotFound();
    else
    {
        var pagedOrderSummaries = await orderSummaries
            .ToPagedResponseAsync(page);
        return Results.Ok(pagedOrderSummaries);
    }
});

app.Run();


