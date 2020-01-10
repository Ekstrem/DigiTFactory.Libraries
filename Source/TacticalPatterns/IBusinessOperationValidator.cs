namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IBusinessOperationValidator<TOperation, TBoundedContext>
        where TOperation : IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}
