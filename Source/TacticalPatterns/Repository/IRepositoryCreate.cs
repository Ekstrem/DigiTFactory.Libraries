using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Функциональность репозитория по созданию записи.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст предметной области.</typeparam>
	public interface IRepositoryCreate<TBoundedContext>
		where TBoundedContext : IBoundedContext
	{
		/// <summary>
		/// Добавляет запись в базу данных.
		/// </summary>
		/// <param name="entity">Сущность хранящаяся в базе данных.</param>
		/// <returns>Идентификатор сущности в базе даннх.</returns>
		void Add(IAggregate<TBoundedContext> entity);
	}
}
