namespace OrderApi.Events;

public interface IEventPublisher
{
  Task PublishAsync<TEvent>(TEvent evt);
}
