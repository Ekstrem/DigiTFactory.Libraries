using System;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: свойство Value Object НЕ должно равняться указанному значению.
    /// DSL: "PropertyNotEquals:VoName.PropertyName:ForbiddenValue"
    /// Null-safe: если VO или свойство == null, проверка считается пройденной
    /// (null не равно никакому значению).
    /// </summary>
    public sealed class PropertyNotEqualsAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;
        private readonly string _forbiddenValue;

        public PropertyNotEqualsAssertion(InvariantMetadata metadata,
            string voName, string propertyName, string forbiddenValue) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
            _forbiddenValue = forbiddenValue;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Aggregate, _voName);
            if (vo == null) return true; // null VO → не равно forbidden value

            var value = vo.Get(_propertyName);
            if (value == null) return true; // null property → не равно forbidden value

            return !string.Equals(value.ToString(), _forbiddenValue, StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class PropertyNotEqualsValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;
        private readonly string _propertyName;
        private readonly string _forbiddenValue;

        public PropertyNotEqualsValidator(InvariantMetadata metadata,
            string voName, string propertyName, string forbiddenValue) : base(metadata)
        {
            _voName = voName;
            _propertyName = propertyName;
            _forbiddenValue = forbiddenValue;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vo = GetDynamicVO(obj.Aggregate, _voName);
            if (vo == null) return true;

            var value = vo.Get(_propertyName);
            if (value == null) return true;

            return !string.Equals(value.ToString(), _forbiddenValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}
