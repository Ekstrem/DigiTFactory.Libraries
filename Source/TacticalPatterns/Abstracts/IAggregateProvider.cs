using System;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    /// <summary>
    /// Провайдер получение экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAggregate">Корневой агрегат контекста.</typeparam>
    public interface IAggregateProvider<TBoundedContext, out TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия аргрегата.</param>
        /// <returns>Коллекция агрегатов удовлетворяющая условию.</returns>
        TAggregate GetAggregate(Guid id, long version);
        
        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="correlationToken">Маркер корреляции.</param>
        /// <returns>Коллекция агрегатов удовлетворяющая условию.</returns>
        TAggregate GetAggregate(Guid correlationToken);
        
        
    }
}
