using System.Linq;

namespace Hive.SeedWorks.Pipelines.Abstractions
{
	/// <summary>
	/// Фильтр доступа.
	/// </summary>
	/// <typeparam name="T">Входящий тип.</typeparam>
	public interface IPermissionFilter<T>
	{
		IQueryable<T> GetPermitted(IQueryable<T> queryable);
	}
}
