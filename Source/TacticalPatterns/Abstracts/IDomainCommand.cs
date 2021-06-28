using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IDomainCommand<TBoundedContext, TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Имя метода агрегата.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Обработчик метода агрегата.
        /// </summary>
        /// <param name="anemicModel">Анемичная модель.</param>
        /// <returns>Результат выполнения метода агрегата.</returns>
        AggregateResultDiff Handle(IAnemicModel<TBoundedContext> anemicModel);
    }
}
