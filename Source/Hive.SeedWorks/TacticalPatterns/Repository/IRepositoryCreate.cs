namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Функциональность репозитория по созданию записи.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryCreate<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
		/// <summary>
		/// Добавляет запись в базу данных.
		/// </summary>
		/// <param name="entity">Сущность хранящаяся в базе данных.</param>
		/// <returns>Идентификатор сущности в базе даннх.</returns>
		void Add(TModel entity);
	}
}
