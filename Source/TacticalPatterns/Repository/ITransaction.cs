using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Управление транзакциями.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Начать транзакцию.
        /// </summary>
        IDisposable Begin();

        /// <summary>
        /// Начать транзакцию асинхронно.
        /// </summary>
        Task<IDisposable> BeginAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Подтвердить транзакцию.
        /// </summary>
        void Commit();

        /// <summary>
        /// Откатить транзакцию.
        /// </summary>
        void RollBack();

        /// <summary>
        /// Установить уровень изоляции.
        /// </summary>
        void SetIsolationLevel(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
