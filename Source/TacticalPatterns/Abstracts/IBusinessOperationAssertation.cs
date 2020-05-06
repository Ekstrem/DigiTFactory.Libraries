namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IBusinessOperationAssertation<TBoundedContext>
        : IBusinessOperationSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}