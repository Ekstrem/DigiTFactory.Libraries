using System.Linq;
using Hive.SeedWorks;
using Hive.SeedWorks.DomainModel;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Fida.Services.Cfs.DomainModel.Result
{
    public class ExceptionAggregateResult : 
        AggregateResultDiff
    {
        private readonly BusinessOperationValidator<TBoundedContext> _assertions;

        internal ExceptionAggregateResult(
            BusinessOperationData businessOperationData,
            params IBusinessOperationAssertation<TBoundedContext>[] assertions)
            : base(businessOperationData)
        {
            _assertions = BusinessOperationValidator<TBoundedContext>
                .Create(businessOperationData.Aggregate, assertions);
        }

        public override DomainOperationResult Result
            => _assertions.Result
                ? DomainOperationResult.Success
                : DomainOperationResult.Exception;

        public override string Reason
            => _assertions
                .GetFailedValidatorsNames()
                .Aggregate((a, i) => $"{a}; {i}");
    }
}