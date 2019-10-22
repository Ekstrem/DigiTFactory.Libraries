namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория команд.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
	/// <typeparam name="TAggregate">Модель для доступа к таблице/таблицам
	/// в базе данных.</typeparam>
	public interface ICommandRepository<TBoundedContext, TAggregate> :
		IRepositoryCreate<TBoundedContext, TAggregate>,
		IRepositoryDelete<TBoundedContext, TAggregate>,
		IRepositoryUpdate<TBoundedContext, TAggregate>
		where TBoundedContext : IBoundedContext
		where TAggregate : class, IAggregateRoot<TBoundedContext>
	{
	}
}
