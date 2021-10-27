using System.Collections.Generic;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;

namespace Hive.SeedWorks.Result
{
    public class AggregateResultSuccess<TBoundedContext> :
        AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public AggregateResultSuccess(BusinessOperationData<TBoundedContext> businessOperationData)
            : base(businessOperationData)
        {
        }

        public override DomainOperationResultEnum Result => DomainOperationResultEnum.Success;

        public override IEnumerable<string> Reason => null;
    }
}