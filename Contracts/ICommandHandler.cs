namespace OrderApi.Contracts;

public interface ICommandHandler<TCommand, TResult> where TCommand : notnull
{
  Task<TResult?> HandlerAsync(TCommand command);
}
