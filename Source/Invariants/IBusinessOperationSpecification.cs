using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Invariants
{
    /// <summary>
    /// Спецификация бизнес операций,
    /// проверяет что результат операции соблюдает инварианты,
    /// а результат можно пременить.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public interface IBusinessOperationSpecification<TBoundedContext, TModel, out TResults>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Проверка соответствия спецификации объекта.
        /// </summary>
        /// <param name="obj">Объект проверки.</param>
        /// <returns>Результат проверки.</returns>
        bool IsSatisfiedBy(BusinessOperationData<TBoundedContext, TModel> obj);
        
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
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public interface IBusinessOperationSpecification<TBoundedContext, TModel> :
        IBusinessOperationSpecification<TBoundedContext, TModel, DomainOperationResultEnum>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
}