namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}
