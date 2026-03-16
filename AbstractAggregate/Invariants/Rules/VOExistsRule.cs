using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules
{
    /// <summary>
    /// Правило: указанный Value Object должен существовать в модели.
    /// DSL: "VOExists:VoName"
    /// </summary>
    public sealed class VOExistsAssertion<TBoundedContext> : MetadataAssertionSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;

        public VOExistsAssertion(InvariantMetadata metadata, string voName) : base(metadata)
        {
            _voName = voName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            // Проверяем текущее состояние (pre-condition): VO должен существовать ДО операции
            var vos = obj.Aggregate.GetValueObjects();
            return vos.TryGetValue(_voName, out var vo) && vo != null;
        }
    }

    public sealed class VOExistsValidator<TBoundedContext> : MetadataValidatorSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _voName;

        public VOExistsValidator(InvariantMetadata metadata, string voName) : base(metadata)
        {
            _voName = voName;
        }

        public override bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj)
        {
            var vos = obj.Aggregate.GetValueObjects();
            return vos.TryGetValue(_voName, out var vo) && vo != null;
        }
    }
}
