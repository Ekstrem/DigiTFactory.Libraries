using System.Collections.Generic;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	/// <typeparam name="TEntity">Тип записи.</typeparam>
	public interface IRepositoryReadEnumerable<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получение всего набора записей.
		/// </summary>
		/// <returns>Набор записей.</returns>
		IEnumerable<TEntity> Get(IPaging page);
	}
}
