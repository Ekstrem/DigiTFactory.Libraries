using System;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: текущее состояние агрегата должно совпадать с указанным.
    /// Состояние ищется в VO по свойству "Status" или "State".
    /// DSL: "StateIs:ExpectedState"
    /// </summary>
    public sealed class StateIsAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _expectedState;

        public StateIsAssertion(InvariantMetadata metadata, string expectedState) : base(metadata)
        {
            _expectedState = expectedState;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            return FindState(obj.Aggregate);
        }

        private bool FindState(IAnemicModel<TBoundedContext> model)
        {
            var vos = model.GetValueObjects();
            foreach (var (_, vo) in vos)
            {
                if (vo is DynamicValueObject dvo)
                {
                    var status = dvo.Get("Status") ?? dvo.Get("State");
                    if (status != null &&
                        string.Equals(status.ToString(), _expectedState, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }

    public sealed class StateIsValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _expectedState;

        public StateIsValidator(InvariantMetadata metadata, string expectedState) : base(metadata)
        {
            _expectedState = expectedState;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vos = obj.Aggregate.GetValueObjects();
            foreach (var (_, vo) in vos)
            {
                if (vo is DynamicValueObject dvo)
                {
                    var status = dvo.Get("Status") ?? dvo.Get("State");
                    if (status != null &&
                        string.Equals(status.ToString(), _expectedState, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }
}
