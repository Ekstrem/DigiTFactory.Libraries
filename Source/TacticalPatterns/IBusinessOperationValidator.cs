namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IBusinessOperationValidator<TBoundedContext> :
        IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        string Name { get; }
    }

    public interface IBusinessOperationValidator<TOperation, TBoundedContext>:
        IBusinessOperationValidator<TBoundedContext>
        where TOperation : IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
