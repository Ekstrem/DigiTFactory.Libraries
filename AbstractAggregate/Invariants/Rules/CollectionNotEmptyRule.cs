using System.Collections;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: коллекция Value Object не должна быть пустой.
    /// DSL: "CollectionNotEmpty:VoName"
    /// </summary>
    public sealed class CollectionNotEmptyAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;

        public CollectionNotEmptyAssertion(InvariantMetadata metadata, string voName) : base(metadata)
        {
            _voName = voName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vos = obj.Model.GetValueObjects();
            if (!vos.TryGetValue(_voName, out var vo) || vo == null) return false;
            if (vo is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext();
            }
            return true; // scalar VO exists — not a collection, treat as satisfied
        }
    }

    public sealed class CollectionNotEmptyValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;

        public CollectionNotEmptyValidator(InvariantMetadata metadata, string voName) : base(metadata)
        {
            _voName = voName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vos = obj.Model.GetValueObjects();
            if (!vos.TryGetValue(_voName, out var vo) || vo == null) return false;
            if (vo is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext();
            }
            return true;
        }
    }
}
