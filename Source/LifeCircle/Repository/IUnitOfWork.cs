using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Единица работы.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
	/// <typeparam name="TAggregate">Модель для доступа к таблице/таблицам
	/// в базе данных.</typeparam>
	public interface IUnitOfWork<TBoundedContext, TAggregate>
        where TBoundedContext : IBoundedContext
		where TAggregate : class, IAggregate<TBoundedContext>
    {
		/// <summary>
		/// Получение репозитория команд.
		/// </summary>
		/// <returns>Репозиторий интересующего типа.</returns>
		ICommandRepository<TBoundedContext, TAggregate> CommandRepository { get; }

		/// <summary>
		/// Получение репозитория запросов.
		/// </summary>
		/// <returns>Репозиторий интересующего типа.</returns>
		IQueryRepository<TAggregate> QueryRepository { get; }

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

        /// <summary>
        /// Управление транзакциями.
        /// </summary>
        ITransaction TransactionManager { get; }
    }
}
