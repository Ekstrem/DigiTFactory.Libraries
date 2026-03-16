using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: свойство Value Object не должно быть null.
    /// DSL: "PropertyNotNull:VoName.PropertyName"
    /// </summary>
    public sealed class PropertyNotNullAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;

        public PropertyNotNullAssertion(InvariantMetadata metadata,
            string voName, string propertyName) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Model, _voName);
            if (vo == null) return false;
            return vo.Get(_propertyName) != null;
        }
    }

    public sealed class PropertyNotNullValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;

        public PropertyNotNullValidator(InvariantMetadata metadata,
            string voName, string propertyName) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Model, _voName);
            if (vo == null) return false;
            return vo.Get(_propertyName) != null;
        }
    }
}
