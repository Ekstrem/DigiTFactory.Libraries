using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Функциональность репозитория по созданию записи.
	/// </summary>
	/// <typeparam name="TAggregate">Тип записи.</typeparam>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryCreate<TBoundedContext, TAggregate>
		where TAggregate : class, IAggregateRoot<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Добавляет запись в базу данных.
		/// </summary>
		/// <param name="entity">Сущность хранящаяся в базе данных.</param>
		/// <returns>Идентификатор сущности в базе даннх.</returns>
		TAggregate Add(TAggregate entity);

		/// <summary>
		/// Добавляет асинхронно запись в базу данных.
		/// </summary>
		/// <param name="entity">Сущность хранящаяся в базе данных.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Идентификатор сущности в базе даннх.</returns>
		Task<TAggregate> AddAsync(
			TAggregate entity,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Добавляет асинхронно записи в базу данных.
		/// </summary>
		/// <param name="entities">Сущности хранящаяся в базе данных.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача добавления новых сущностей.</returns>
		Task AddRangeAsync(
			IEnumerable<TAggregate> entities,
			CancellationToken cancellationToken = default);
	}
}
