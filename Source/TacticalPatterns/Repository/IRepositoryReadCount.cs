using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	public interface IRepositoryReadCount<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <returns>Возвращает количество элементов данных.</returns>
		long Count();

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает количество элементов данных.</returns>
		Task<long> CountAsync(CancellationToken cancellationToken = default);
	}
}
