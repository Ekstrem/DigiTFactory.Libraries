using System.Collections.Generic;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    public class AggregateResultSuccess<TBoundedContext, TModel> :
        AggregateResult<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public AggregateResultSuccess(BusinessOperationData<TBoundedContext, TModel> businessOperationData)
            : base(businessOperationData)
        {
        }

        public override DomainOperationResultEnum Result => DomainOperationResultEnum.Success;

        public override IEnumerable<string> Reason => null;
    }
}