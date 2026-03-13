using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    public interface IBusinessEntityValidator<TBoundedContext>
        : IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
