using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: агрегат должен уже существовать (Version > 0).
    /// DSL: "IsNotNewEntity"
    /// </summary>
    public sealed class IsNotNewEntityAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public IsNotNewEntityAssertion(InvariantMetadata metadata) : base(metadata) { }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            return obj.Aggregate.Version > 0;
        }
    }

    public sealed class IsNotNewEntityValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public IsNotNewEntityValidator(InvariantMetadata metadata) : base(metadata) { }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            return obj.Aggregate.Version > 0;
        }
    }
}
