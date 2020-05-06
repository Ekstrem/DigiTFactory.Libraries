using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal
{
	/// <summary>
	/// Репозиторий запросов.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
	/// <typeparam name="TModel">Анемичная модель.</typeparam>
	public class QueryRepository<TBoundedContext, TModel> :
		BaseRepository, IQueryRepository<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
		/// <summary>
		/// Конструктор репозитория.
		/// </summary>
		/// <param name="context">Контекст подключения к базе данных.</param>
		public QueryRepository(DbContext context)
			: base(context)
		{
		}

		public IQueryable<TModel> GetQueryable()
		{
			return GetSet<TModel>().AsNoTracking();
		}

		/// <summary>Получение страницы записей.</summary>
		/// <param name="pagу">Паджинация запроса.</param>
		/// <returns>Коллекция моделей.</returns>
		public IEnumerable<TModel> Get(IPaging page)
		{
			return page == default
				? GetSet<TModel>()
					.AsEnumerable()
				: GetSet<TModel>()
					.Skip(page.PageSize * page.Page)
					.Take(page.PageSize)
					.AsEnumerable();
		}

		/// <summary>
		/// Получить колличество записей.
		/// </summary>
		/// <returns>Число записей.</returns>
		public long Count()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Получить колличество записей асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Число записей.</returns>
		public Task<long> CountAsync(CancellationToken cancellationToken = default)
		{
			return GetSet<TModel>().LongCountAsync(cancellationToken);
		}
	}
}
