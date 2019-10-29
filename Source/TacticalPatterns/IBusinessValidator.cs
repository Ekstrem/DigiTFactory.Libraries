namespace Hive.SeedWorks.LifeCircle
{
    public interface IBusinessValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}