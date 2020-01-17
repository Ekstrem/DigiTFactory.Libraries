namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория обновления.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryUpdate<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
		/// <summary>
		/// Обновление сущностей в базе данных.
		/// </summary>
		/// <param name="entity">Сущность для обновления в базе данных.</param>
		void Update(TModel entity);
	}
}
