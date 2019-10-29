namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория обновления.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryUpdate<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Обновление сущностей в базе данных.
		/// </summary>
		/// <param name="entity">Сущность для обновления в базе данных.</param>
		void Update(IAnemicModel<TBoundedContext> entity);
	}
}
