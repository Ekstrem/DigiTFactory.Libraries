namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория запросов.
	/// </summary>
	public interface IQueryRepository<TBoundedContext> :
		IRepositoryReadCount<TBoundedContext>,
		IRepositoryReadEnumerable<TBoundedContext>,
		IRepositoryReadQueryable<TBoundedContext>,
		IRepositoryReadSingle<TBoundedContext>
	{
	}
}
