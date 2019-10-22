namespace Hive.SeedWorks.Pipelines.Abstractions.Handlers
{
	/// <summary>
	/// Обработчик запросов.
	/// </summary>
	/// <typeparam name="TIn">Тип для запроса.</typeparam>
	/// <typeparam name="TOut">Тип результата запроса.</typeparam>
	public interface IQueryHandler<in TIn, out TOut>
		: IHandler<TIn, TOut>
		where TIn : IQuery<TOut> { }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    /// <typeparam name="TIn">Тип для запроса.</typeparam>
    /// <typeparam name="TOut">Тип результата запроса.</typeparam>
    public interface IQueryHandlerAsync<in TIn, TOut>
        : IHandlerAsync<TIn, TOut>
        where TIn : IQuery<TOut>
    { }
}
