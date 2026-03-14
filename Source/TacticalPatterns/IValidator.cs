using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    public interface IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        bool ValidateModel(IAnemicModel<TBoundedContext> anemicModel);
    }
}
