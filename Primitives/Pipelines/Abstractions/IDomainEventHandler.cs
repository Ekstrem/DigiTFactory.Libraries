using System.Threading.Tasks;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.LifeCircle;
using Hive.SeedWorks.Pipelines.Abstractions.Handlers;

namespace Hive.SeedWorks.Pipelines.Abstractions
{
    /// <summary>
    /// Обработчик доменного события.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public interface IDomainEventHandler<TBoundedContext>
        : IHandler<IDomainEvent<TBoundedContext>, Task>
        where TBoundedContext : IBoundedContext
    {

    }
}