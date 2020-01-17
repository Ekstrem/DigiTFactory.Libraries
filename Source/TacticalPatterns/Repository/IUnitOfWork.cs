using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Единица работы.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
	public interface IUnitOfWork<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
		/// <summary>
		/// Получение репозитория команд.
		/// </summary>
		/// <returns>Репозиторий интересующего типа.</returns>
		ICommandRepository<TBoundedContext> CommandRepository { get; }

		/// <summary>
		/// Получение репозитория запросов.
		/// </summary>
		/// <returns>Репозиторий интересующего типа.</returns>
		IQueryRepository<TBoundedContext> QueryRepository { get; }

        /// <summary>
        /// Сохранение результатов.
        /// </summary>
        /// <returns>Количество изменённых записей.</returns>
        int Save();

        /// <summary>
        /// Асинхронное сохранение результатов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Продолжение с количеством изменнёх записей.</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Вызов хранимых процедур.
        /// </summary>
        ISqlProgrammability Programmability { get; }
    }
}
