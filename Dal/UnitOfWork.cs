using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Repository;
using Hive.Dal.RawSql;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Dal
{
    /// <summary>
    /// Единица работы.
    /// </summary>
    public class UnitOfWork<TBoundedContext, TModel> : IUnitOfWork<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
		protected readonly DbContext _context;
		private readonly ICommandRepository<TBoundedContext, TModel> _commandRepository;
		private readonly IQueryRepository<TBoundedContext, TModel> _queryRepository;

		/// <summary>
		/// Конструктор внедрения зависимостей.
		/// </summary>
		/// <param name="context">Контект базы данных.</param>
		public UnitOfWork(DbContext context)
			: this(
				context,
				Transaction.Fabric(context),
				new SqlProgrammability(context))
		{
		}

		/// <summary>
		/// Конструктор внедрения зависимостей.
		/// </summary>
		/// <param name="context">Контект базы данных.</param>
		/// <param name="transactionManager">Управление транзакциями.</param>
		/// <param name="programmability">Доступ к хранимым процедурам.</param>
		public UnitOfWork(
			DbContext context,
			ITransaction transactionManager,
			ISqlProgrammability programmability)
		{
			_context = context;
			_commandRepository = new CommandRepository<TBoundedContext, TModel>(context);
			_queryRepository = new QueryRepository<TBoundedContext, TModel>(context);

			TransactionManager = transactionManager;
			Programmability = programmability;
		}

		/// <summary>
		/// Получение репозитория комманд.
		/// </summary>
		/// <returns>Репозиторий для доступа к БД.</returns>
		public ICommandRepository<TBoundedContext, TModel> CommandRepository
			=> _commandRepository;

		/// <summary>
		/// Получение репозитория запросов.
		/// </summary>
		/// <returns>Репозиторий для доступа к БД.</returns>
		public IQueryRepository<TBoundedContext, TModel> QueryRepository
			=> _queryRepository;

        /// <summary>Сохранение результатов.</summary>
		/// <returns>Количество изменённых записей.</returns>
		public int Save()
		{
			return _context.SaveChanges();
		}

		/// <summary>Асинхронное сохранение результатов</summary>
		/// <param name="cancellationToken"></param>
		/// <returns>Продолжение с количеством изменнёх записей.</returns>
		public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
		{
			return await _context.SaveChangesAsync(cancellationToken);
		}

        /// <summary>Вызов хранимых процедур.</summary>
        public ISqlProgrammability Programmability { get; protected set; }

        /// <summary>Управление транзакциями.</summary>
        public ITransaction TransactionManager { get; protected set; }
	}
}
