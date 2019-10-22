using System.Collections.Generic;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Specification;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория удаления.
	/// </summary>
	/// <typeparam name="TAggregate">Таблица в базе данных.</typeparam>
	/// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
	public interface IRepositoryDelete<TBoundedContext, TAggregate>
		where TAggregate : class, IAggregateRoot<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Удаление записи из базы данных.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация для удаления из базы данных.</param>
		IEnumerable<TAggregate> Delete<TIn>(
			TIn value,
			IQuerySpecification<TIn, TAggregate> specification)
			where TIn : IQuery;

		/// <summary>
		/// Удаление записи из базы данных.
		/// </summary>
		/// <param name="entity">Сущность для удаления из базы данных.</param>
		TAggregate Delete(TAggregate entity);
	}
}
