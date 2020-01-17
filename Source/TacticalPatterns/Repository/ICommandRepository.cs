namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория команд.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
	public interface ICommandRepository<TBoundedContext, TModel> :
		IRepositoryCreate<TBoundedContext, TModel>,
		IRepositoryUpdate<TBoundedContext, TModel>,
		IRepositoryDelete<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
	}
}
