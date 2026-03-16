using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants
{
    /// <summary>
    /// Базовый класс спецификации, построенной из метаданных.
    /// Наследники реализуют конкретную логику проверки.
    /// </summary>
    public abstract class MetadataAssertionSpecification<TBoundedContext> :
        IBusinessOperationAssertion<TBoundedContext, IAnemicModel<TBoundedContext>>
        where TBoundedContext : IBoundedContext
    {
        protected MetadataAssertionSpecification(InvariantMetadata metadata)
        {
            Reason = metadata.FailureReason;
            DomainResult = metadata.FailureResult;
        }

        /// <inheritdoc />
        public string Reason { get; }

        /// <inheritdoc />
        public DomainOperationResultEnum DomainResult { get; }

        /// <inheritdoc />
        public abstract bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj);

        /// <summary>
        /// Вспомогательный метод: получить DynamicValueObject из модели.
        /// </summary>
        protected static DynamicValueObject? GetDynamicVO(
            IAnemicModel<TBoundedContext> model, string voName)
        {
            var vos = model.GetValueObjects();
            return vos.TryGetValue(voName, out var vo) ? vo as DynamicValueObject : null;
        }
    }

    /// <summary>
    /// Базовый класс спецификации-валидатора (Warning при нарушении).
    /// </summary>
    public abstract class MetadataValidatorSpecification<TBoundedContext> :
        IBusinessOperationValidator<TBoundedContext, IAnemicModel<TBoundedContext>>
        where TBoundedContext : IBoundedContext
    {
        protected MetadataValidatorSpecification(InvariantMetadata metadata)
        {
            Reason = metadata.FailureReason;
            DomainResult = metadata.FailureResult;
        }

        /// <inheritdoc />
        public string Reason { get; }

        /// <inheritdoc />
        public DomainOperationResultEnum DomainResult { get; }

        /// <inheritdoc />
        public abstract bool IsSatisfiedBy(
            BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>> obj);

        /// <summary>
        /// Вспомогательный метод: получить DynamicValueObject из модели.
        /// </summary>
        protected static DynamicValueObject? GetDynamicVO(
            IAnemicModel<TBoundedContext> model, string voName)
        {
            var vos = model.GetValueObjects();
            return vos.TryGetValue(voName, out var vo) ? vo as DynamicValueObject : null;
        }
    }
}
