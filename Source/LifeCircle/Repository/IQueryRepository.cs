namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория запросов.
	/// </summary>
	/// <typeparam name="TEntity">Модель для доступа к таблице/таблицам
	/// в базе данных.</typeparam>
	public interface IQueryRepository<TEntity> :
		IRepositoryReadCount<TEntity>,
		IRepositoryReadEnumerable<TEntity>,
		IRepositoryReadQueryable<TEntity>,
		IRepositoryReadSingle<TEntity>
		where TEntity : class, IEntity
	{
	}

	/// <summary>
	/// Интерфейс репозитория запросов.
	/// </summary>
	public interface IQueryRepository :
		IRepositoryReadCount,
		IRepositoryReadEnumerable,
		IRepositoryReadQueryable,
		IRepositoryReadSingle
	{
	}
}
