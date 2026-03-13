using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Агрегат, предоставляющий доступ к бизнес-операциям ограниченного контекста.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class Aggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Бизнес-операции агрегата.
        /// </summary>
        public IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> Operations { get; }

        private Aggregate(IBoundedContextScope<TBoundedContext> scope)
        {
            Operations = scope.Operations;
        }

        /// <summary>
        /// Создать экземпляр агрегата.
        /// </summary>
        /// <param name="model">Анемичная модель.</param>
        /// <param name="scope">Область ограниченного контекста.</param>
        /// <returns>Экземпляр агрегата.</returns>
        public static Aggregate<TBoundedContext> CreateInstance(
            IAnemicModel<TBoundedContext> model,
            IBoundedContextScope<TBoundedContext> scope)
        {
            return new Aggregate<TBoundedContext>(scope);
        }
    }
}
