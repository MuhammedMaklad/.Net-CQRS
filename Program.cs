using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Filters;
using OrderApi;
using OrderApi.Commands;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Events;
using OrderApi.Handlers;
using OrderApi.Models;
using OrderApi.Projections;
using OrderApi.Queries;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection"));
// });
builder.Services.AddDbContext<WriteDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("WriteDbConnection"));
});
builder.Services.AddDbContext<ReadDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ReadDbConnection"));
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ! register automapper 
builder.Services.AddAutoMapper(typeof(OrderProfile));

// ! register services
// builder.Services.AddScoped<ICommandHandler<CreateOrderCommand,OrderDto>, CreateOrderCommandHandler>();
// builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
// builder.Services.AddScoped<IQueryHandler<GetOrderSummaryQuery, List<OrderSummaryDto>>, GetOrderSummaryQueryHandler>();
// builder.Services.AddSingleton<IEventPublisher, InProgressEventPublisher>();
// builder.Services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedProjectionHandler>();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// ! register validation
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// register middleware
app.UseMiddleware<ExceptionMiddleware>();


//! ----------- End Points ------------ \\

// app.MapGet("/api/orders/{id:int}", async (AppDbContext context, int id) =>
// {
//     return await context.Orders.SingleOrDefaultAsync(x => x.Id == id);
// });

// app.MapPost("/api/orders", async (AppDbContext context, Order order) =>
// {
//     await context.Orders.AddAsync(order);
//     await context.SaveChangesAsync();

//     return Results.Created($"/api/orders{order.Id}", order);
// });

// ---- using CQRS -------------- \\

// app.MapGet("/api/cqrs/v1/orders/{id:int}", async (AppDbContext context, int id) =>
// {
//     var order = await GetOrderByIdQueryHandler.Handle(new GetOrderByIdQuery(id), context);
//     if (order is null)
//         return Results.NotFound();
//     return Results.Ok(order);
// });

// app.MapPost("/api/cqrs/v1/orders", async (AppDbContext context, CreateOrderCommand command) =>
// {
//     var order = await CreateOrderCommandHandler.Handle(command, context);
//     if (order is null)
//         return Results.Problem(detail:"Error while create order", statusCode: StatusCodes.Status500InternalServerError);

//     return Results.Created($"/api/cqrs/v1/orders{order.Id}", order);
// });

// app.MapGet("/api/cqrs/v2/orders/{id:int}", async (IQueryHandler<GetOrderByIdQuery, OrderDto> queryHandler,  int id) =>
// {
//     return await queryHandler.HandlerAsync(new GetOrderByIdQuery(id));
    
// });

// app.MapPost("/api/cqrs/v2/orders", async (ICommandHandler<CreateOrderCommand, OrderDto> commandHandler, CreateOrderCommand command) =>
// {
//     var order = await commandHandler.HandlerAsync(command);
//     if (order is null)
//         return Results.Problem(detail: "Error while create order", statusCode: StatusCodes.Status500InternalServerError);

//     return Results.Created($"/api/cqrs/v2/orders{order.Id}", order);
// })
// .AddEndpointFilter<ValidationFilter<CreateOrderCommand>>();


// app.MapGet("/api/cqrs/v2/orders", async (IQueryHandler<GetOrderSummaryQuery, List<OrderSummaryDto>> queryHandler) =>
// {
//     var orders = await queryHandler.HandlerAsync(new GetOrderSummaryQuery());
//     return Results.Ok(orders);
// });

app.MapGet("/api/cqrs/v2/orders/{id:int}", async (IMediator mediator,  int id) =>
{
    return await mediator.Send(new GetOrderByIdQuery(id));
    
});

app.MapPost("/api/cqrs/v2/orders", async (IMediator mediator, CreateOrderCommand command) =>
{
    var order = await mediator.Send(command);
    if (order is null)
        return Results.Problem(detail: "Error while create order", statusCode: StatusCodes.Status500InternalServerError);

    return Results.Created($"/api/cqrs/v2/orders{order.Id}", order);
})
.AddEndpointFilter<ValidationFilter<CreateOrderCommand>>();


app.MapGet("/api/cqrs/v2/orders", async (IMediator mediator) =>
{
    var orders = await mediator.Send(new GetOrderSummaryQuery());
    return Results.Ok(orders);
});


app.UseHttpsRedirection();



app.Run();
