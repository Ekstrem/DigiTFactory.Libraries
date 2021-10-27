using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.Invariants
{
    /// <summary>
    /// Спецификация бизнес операций,
    /// проверяет что результат операции соблюдает инварианты,
    /// а результат можно пременить.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    public interface IBusinessOperationSpecification<TBoundedContext, out TResults>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Проверка соответствия спецификации объекта.
        /// </summary>
        /// <param name="obj">Объект проверки.</param>
        /// <returns>Результат проверки.</returns>
        bool IsSatisfiedBy(BusinessOperationData<TBoundedContext> obj);
        
        /// <summary>
        /// Причина, по которой объект не проходит проверку данной спецификацией.
        /// </summary>
        string Reason { get; }
        
        /// <summary>
        /// Результат бизнес-операции, в случае не пройденной спецификации.
        /// </summary>
        TResults DomainResult { get; }
    }
    
    /// <summary>
    /// Спецификация бизнес операций,
    /// проверяет что результат операции соблюдает инварианты,
    /// а результат можно пременить.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public interface IBusinessOperationSpecification<TBoundedContext> :
        IBusinessOperationSpecification<TBoundedContext, DomainOperationResultEnum>
        where TBoundedContext : IBoundedContext
    { }
}