using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Интерфейс репозитория. Функциональность чтения.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IQueryRepository<TBoundedContext>
		where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Получение записей без спецификации.
        /// </summary>
        /// <returns>Коллекция записей.</returns>
        IQueryable<IAnemicModel<TBoundedContext>> GetQueryable();

        /// <summary>
        /// Получение всего набора записей.
        /// </summary>
        /// <returns>Набор записей.</returns>
        IEnumerable<IEntity> Get(IPaging page);

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
