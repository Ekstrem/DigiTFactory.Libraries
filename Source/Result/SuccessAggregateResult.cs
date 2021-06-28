using Hive.SeedWorks.DomainModel;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.Result
{
    public class SuccessAggregateResult : AggregateResultDiff
    {
        public SuccessAggregateResult(
            BusinessOperationData businessOperationData)
            : base(businessOperationData)
        {
        }

        public override DomainOperationResult Result => DomainOperationResult.Success;

        public override string Reason => null;
    }
}