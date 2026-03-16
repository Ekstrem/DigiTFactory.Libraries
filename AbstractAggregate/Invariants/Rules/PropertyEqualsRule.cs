using System;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: свойство Value Object должно равняться указанному значению.
    /// DSL: "PropertyEquals:VoName.PropertyName:ExpectedValue"
    /// </summary>
    public sealed class PropertyEqualsAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;
        private readonly string _expectedValue;

        public PropertyEqualsAssertion(InvariantMetadata metadata,
            string voName, string propertyName, string expectedValue) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
            _expectedValue = expectedValue;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Model, _voName);
            if (vo == null) return false;

            var value = vo.Get(_propertyName);
            return string.Equals(value?.ToString(), _expectedValue, StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class PropertyEqualsValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;
        private readonly string _expectedValue;

        public PropertyEqualsValidator(InvariantMetadata metadata,
            string voName, string propertyName, string expectedValue) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
            _expectedValue = expectedValue;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Model, _voName);
            if (vo == null) return false;

            var value = vo.Get(_propertyName);
            return string.Equals(value?.ToString(), _expectedValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}
