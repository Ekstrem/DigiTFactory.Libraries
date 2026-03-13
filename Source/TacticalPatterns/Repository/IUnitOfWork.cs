using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Единица работы (Unit of Work).
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TModel">Анемичная модель.</typeparam>
    public interface IUnitOfWork<TBoundedContext, TModel>
        where TBoundedContext : IBoundedContext
        where TModel : AnemicModel<TBoundedContext>
    {
        /// <summary>
        /// Репозиторий команд.
        /// </summary>
        ICommandRepository<TBoundedContext, TModel> CommandRepository { get; }

        /// <summary>
        /// Репозиторий запросов.
        /// </summary>
        IQueryRepository<TBoundedContext, TModel> QueryRepository { get; }

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        int Save();

        /// <summary>
        /// Сохранить изменения асинхронно.
        /// </summary>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Доступ к хранимым процедурам.
        /// </summary>
        ISqlProgrammability Programmability { get; }

        /// <summary>
        /// Управление транзакциями.
        /// </summary>
        ITransaction TransactionManager { get; }
    }
}
