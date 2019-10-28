namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория команд.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
	public interface ICommandRepository<TBoundedContext> :
		IRepositoryCreate<TBoundedContext>,
		IRepositoryDelete<TBoundedContext>,
		IRepositoryUpdate<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
	}
}
