using Hive.SeedWorks.Definition;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Invariants
{
    /// <summary>
    /// Спецификация проверки на возможность проведения бизнес операции.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public interface IBusinessOperationValidator<TBoundedContext, TModel, out TResults> :
        IBusinessOperationSpecification<TBoundedContext, TModel, TResults>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
    
    /// <summary>
    /// Спецификация проверки на возможность проведения бизнес операции.
    /// </summary>
    /// <typeparam name="TBoundedContext"></typeparam>
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public interface IBusinessOperationValidator<TBoundedContext, TModel> :
        IBusinessOperationSpecification<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
}