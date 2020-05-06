namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IBusinessOperationValidator<TBoundedContext> :
        IBusinessOperationSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
