using System;
using System.Linq;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	/// <typeparam name="TEntity">Тип записи.</typeparam>
	public interface IRepositoryReadQueryable<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получение записей без спецификации.
		/// </summary>
		/// <returns>Коллекция записей.</returns>
		IQueryable<TEntity> GetQueryable();
	}
}
