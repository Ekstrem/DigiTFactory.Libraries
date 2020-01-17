using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Интерфейс репозитория. Функциональность чтения.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IQueryRepository<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
        /// <summary>
        /// Получение записей без спецификации.
        /// </summary>
        /// <returns>Коллекция записей.</returns>
        IQueryable<TModel> GetQueryable();

        /// <summary>
        /// Получение всего набора записей.
        /// </summary>
        /// <returns>Набор записей.</returns>
        IEnumerable<TModel> Get(IPaging page);

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
