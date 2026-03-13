using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Репозиторий запросов (Read).
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TModel">Анемичная модель.</typeparam>
    public interface IQueryRepository<TBoundedContext, TModel>
        where TBoundedContext : IBoundedContext
        where TModel : AnemicModel<TBoundedContext>
    {
        /// <summary>
        /// Получить IQueryable для произвольных запросов.
        /// </summary>
        IQueryable<TModel> GetQueryable();

        /// <summary>
        /// Получить страницу записей.
        /// </summary>
        IEnumerable<TModel> Get(IPaging page);

        /// <summary>
        /// Получить количество записей.
        /// </summary>
        long Count();

        /// <summary>
        /// Получить количество записей асинхронно.
        /// </summary>
        Task<long> CountAsync(CancellationToken cancellationToken = default);
    }
}
