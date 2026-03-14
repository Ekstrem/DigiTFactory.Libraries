using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Бизнес-операция над агрегатом.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Выполнить бизнес-операцию.
        /// </summary>
        /// <param name="model">Анемичная модель.</param>
        /// <param name="command">Команда к агрегату.</param>
        /// <param name="scope">Область ограниченного контекста.</param>
        /// <returns>Результат выполнения бизнес-операции.</returns>
        AggregateResult<TBoundedContext, IAnemicModel<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> model,
            CommandToAggregate command,
            IBoundedContextScope<TBoundedContext> scope);
    }
}
