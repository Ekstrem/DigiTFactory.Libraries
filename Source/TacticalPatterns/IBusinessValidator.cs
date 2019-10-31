namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IBusinessValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}