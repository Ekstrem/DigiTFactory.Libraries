using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IBusinessEntityValidator<TBoundedContext>
        : IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
