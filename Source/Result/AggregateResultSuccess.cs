using System.Collections.Generic;
using System.Linq;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Result
{
    public sealed class AggregateResultSuccess<TBoundedContext, TModel> :
        AggregateResult<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        public AggregateResultSuccess(BusinessOperationData<TBoundedContext, TModel> businessOperationData)
            : base(businessOperationData)
        {
        }

        public override DomainOperationResultEnum Result => DomainOperationResultEnum.Success;

        public override IEnumerable<string> Reason => Enumerable.Empty<string>();
    }
}