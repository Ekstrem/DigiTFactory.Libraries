using System.Collections.Generic;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Pipelines.Abstractions.Handlers;

namespace Hive.SeedWorks.Pipelines {
	/// <summary>
	/// Обработчик запроса данных.
	/// </summary>
	/// <typeparam name="TQuery">Запрос к БД.</typeparam>
	/// <typeparam name="TEntity">Сущность в БД.</typeparam>
	/// <typeparam name="TProjection">Проекция.</typeparam>
	public abstract class LinqQueryHandler<TQuery, TEntity, TProjection>
		: IQueryHandler<TQuery, IEnumerable<TProjection>>
		where TQuery
		: IQuery<IEnumerable<TProjection>>,
		IFilter<TProjection>,
		ISorter<TProjection>
		where TEntity : class
	{
		public abstract IEnumerable<TProjection> Handle(TQuery input);
	}
}
