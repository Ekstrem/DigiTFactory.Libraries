using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Hive.SeedWorks.LifeCircle.Repository
{
    /// <summary>
    /// Управление транзакционностью.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Управляет поведением блокировки и версиями строк инструкций.
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции.</param>
        void SetIsolationLevel(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Отмечает начальную точку явной локальной транзакции.
        /// </summary>
        IDisposable Begin();

        /// <summary>
        /// Асинхронно отмечает начальную точку явной локальной транзакции.
        /// </summary>
        Task<IDisposable> BeginAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Использование внешней транзакции.
        /// </summary>
        /// <param name="transaction">Внешняя транзакция.</param>
        void UseTransaction(DbTransaction transaction);

        /// <summary>
        /// Отмечает успешное завершение явной или неявной транзакции.
        /// </summary>
        void Commit();

        /// <summary>
        /// Откатывает явные или неявные транзакции до начала или до точки сохранения транзакции.
        /// </summary>
        void RollBack();
    }
}
