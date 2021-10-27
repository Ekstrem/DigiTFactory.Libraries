using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.Invariants
{
    /// <summary>
    /// Спецификация бизнес операций, проверяющая что результат операции не противоречив,
    /// а результат нельзя применить.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    public interface IBusinessOperationAssertion<TBoundedContext, out TResults> :
        IBusinessOperationSpecification<TBoundedContext, TResults>
        where TBoundedContext : IBoundedContext
    { }
    
    /// <summary>
    /// Спецификация бизнес операций, проверяющая что результат операции не противоречив,
    /// а результат нельзя применить.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    public interface IBusinessOperationAssertion<TBoundedContext> :
        IBusinessOperationSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
}