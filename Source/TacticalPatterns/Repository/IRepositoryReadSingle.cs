using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Репозиторий получения единственного агрегата.
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности.</typeparam>
	public interface IRepositoryReadSingle<TEntity>
		where TEntity : class, IEntity
	{
	}
}
