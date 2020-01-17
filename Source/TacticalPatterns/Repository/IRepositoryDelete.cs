namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория удаления.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
	public interface IRepositoryDelete<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Удаление записи из базы данных.
		/// </summary>
		/// <param name="entity">Сущность для удаления из базы данных.</param>
		void Delete(AnemicModel<TBoundedContext> entity);
	}
}
