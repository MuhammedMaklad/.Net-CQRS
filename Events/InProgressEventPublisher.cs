
namespace OrderApi.Events;

public class InProgressEventPublisher : IEventPublisher
{
  private readonly IServiceProvider serviceProvider;
  public InProgressEventPublisher(IServiceProvider serviceProvider)
  {
    this.serviceProvider = serviceProvider;
  }
  public async Task PublishAsync<TEvent>(TEvent evt)
  {
    using var scope = serviceProvider.CreateScope();
    var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
    foreach (var handler in handlers)
    {
      await handler.HandleAsync(evt);
    }
  }
}
