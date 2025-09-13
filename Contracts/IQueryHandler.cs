namespace OrderApi.Contracts;

public interface IQueryHandler<TQuery, TResult>
{
  Task<TResult?> HandlerAsync(TQuery query);
}
