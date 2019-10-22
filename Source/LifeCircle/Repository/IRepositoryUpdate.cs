namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория обновления.
	/// </summary>
	/// <typeparam name="TAggregate">Таблица в базе данных.</typeparam>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryUpdate<TBoundedContext, in TAggregate>
		where TAggregate : class, IAggregateRoot<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Обновление сущностей в базе данных.
		/// </summary>
		/// <param name="entity">Сущность для обновления в базе данных.</param>
		void Update(TAggregate entity);
	}
}
