using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.Invariants
{
    /// <summary>
    /// Спецификация проверки на возможность проведения бизнес операции.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    public interface IBusinessOperationValidator<TBoundedContext, out TResults> :
        IBusinessOperationSpecification<TBoundedContext, TResults>
        where TBoundedContext : IBoundedContext
    { }
    
    /// <summary>
    /// Спецификация проверки на возможность проведения бизнес операции.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    public interface IBusinessOperationValidator<TBoundedContext> :
        IBusinessOperationSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
}