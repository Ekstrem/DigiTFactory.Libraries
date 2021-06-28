using System.Linq;
using Hive.SeedWorks;
using Hive.SeedWorks.DomainModel;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Fida.Services.Cfs.DomainModel.Result
{
    public class InvalidAggregateResult<TBoundedContext, TAnemicModel> : 
        AggregateResultDiff<TBoundedContext, TAnemicModel> 
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly BusinessOperationValidator<TBoundedContext> _validators;

        internal InvalidAggregateResult(
            BusinessOperationData businessOperationData,
            params IBusinessOperationValidator<TBoundedContext>[] validators)
            : base(businessOperationData)
        {
            _validators = BusinessOperationValidator<TBoundedContext>
                .Create(businessOperationData.Command, validators);
        }

        public override DomainOperationResult Result
            => _validators.Result
                ? DomainOperationResult.Success
                : DomainOperationResult.Invalid;

        public override string Reason
            => _validators
                .GetFailedValidatorsNames()
                .Aggregate((a, i) => $"{a}; {i}");
    }
}