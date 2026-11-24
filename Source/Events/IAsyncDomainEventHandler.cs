using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.Events
{
    /// <summary>
    /// Durable-подписчик доменных событий. Шина обязана дождаться <see cref="HandleAsync"/>
    /// до завершения PublishAsync. Используется для записи в Event Store; MV-проекции остаются
    /// асинхронными IObserver-подписчиками и не блокируют команду.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IAsyncDomainEventHandler<TBoundedContext> : IDomainEventHandler<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>Обрабатывает событие и завершается после durable-записи.</summary>
        /// <param name="domainEvent">Доменное событие.</param>
        Task HandleAsync(IDomainEvent<TBoundedContext> domainEvent);
    }
}
