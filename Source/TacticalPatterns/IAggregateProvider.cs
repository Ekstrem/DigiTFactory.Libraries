using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IAggregateProvider<TBoundedContext> :
        IAggregateProvider<TBoundedContext, IAggregate<TBoundedContext>>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAnemicModel">Корневой агрегат контекста.</typeparam>
    public interface IAggregateProvider<TBoundedContext, TAnemicModel> 
        where TAnemicModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <returns>Аггрегат.</returns>
        TAnemicModel GetAggregate(Guid id, long version);


        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Задача на извлечение агрегата.</returns>
        Task<TAnemicModel> GetAggregateAsync(Guid id, long version, CancellationToken cancellationToken);

        /// <summary>
        /// Получение агрегата(ов) по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <returns>Агрегат.</returns>
        TAnemicModel GetAggregate(Guid id);
        
        /// <summary>
        /// Получение агрегата(ов) по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Задача на извлечение агрегата.</returns>
        Task<TAnemicModel> GetAggregateAsync(Guid id, CancellationToken cancellationToken);
    }
}
