namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}
