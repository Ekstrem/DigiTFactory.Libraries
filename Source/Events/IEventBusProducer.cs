using System.Threading.Tasks;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Поставшик в шину событий.
    /// </summary>
    public interface IEventBusProducer
    {
        /// <summary>
        /// Опубликовать доменное событие.
        /// </summary>
        /// <param name="domainEvent">Доменное событие.</param>
        void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)

            where TBoundedContext : IBoundedContext;
        /// <summary>
        /// Опубликовать доменное событие.
        /// </summary>
        /// <param name="domainEvent">Доменное событие.</param>
        Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext;
    }
}