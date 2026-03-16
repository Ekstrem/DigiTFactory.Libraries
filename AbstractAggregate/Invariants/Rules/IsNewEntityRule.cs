using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: агрегат должен быть новым (Version == 0).
    /// DSL: "IsNewEntity"
    /// </summary>
    public sealed class IsNewEntityAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public IsNewEntityAssertion(InvariantMetadata metadata) : base(metadata) { }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            return obj.Aggregate.Version == 0;
        }
    }

    /// <summary>
    /// Правило-валидатор: агрегат должен быть новым (Version == 0).
    /// </summary>
    public sealed class IsNewEntityValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public IsNewEntityValidator(InvariantMetadata metadata) : base(metadata) { }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            return obj.Aggregate.Version == 0;
        }
    }
}
