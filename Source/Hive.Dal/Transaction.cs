using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal
{
	/// <summary>
	/// Управление транзакционностью.
	/// </summary>
	public class Transaction : ITransaction
	{
		private readonly DbContext _context;

		protected Transaction(DbContext context)
		{
			_context = context;
		}

		public IDbContextTransaction Begin()
			=> _context.Database.BeginTransaction();

		/// <summary>
		/// Асинхронно отмечает начальную точку явной локальной транзакции.
		/// </summary>
		public Task<IDbContextTransaction> BeginAsync(
			CancellationToken cancellationToken = default)
			=> _context.Database.BeginTransactionAsync(cancellationToken);

		/// <summary>
		/// Использование внешней транзакции.
		/// </summary>
		/// <param name="transaction">Внешняя транзакция.</param>
		public void UseTransaction(DbTransaction transaction)
			=> _context.Database.UseTransaction(transaction);

		public void Commit() => _context.Database.CommitTransaction();

	    public void RollBack() => _context.Database.RollbackTransaction();

		public void SetIsolationLevel(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) { }

		public static ITransaction Fabric(DbContext context) => new Transaction(context);

        IDisposable ITransaction.Begin()
        {
            throw new NotImplementedException();
        }

        Task<IDisposable> ITransaction.BeginAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}