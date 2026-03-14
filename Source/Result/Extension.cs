using System.Linq;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Monads;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Result
{
    public static class Extension
    {
        public static AggregateResult<TBoundedContext, TModel> ValidateCommand<TBoundedContext, TModel>(
            this BusinessOperationData<TBoundedContext, TModel> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TModel>[] specifications)
            where TModel : IAnemicModel<TBoundedContext>
            where TBoundedContext : IBoundedContext
        {
            var invalid = specifications
                .Where(f => f.GetType().GetInterfaces()
                    .Contains(typeof(IBusinessOperationAssertion<TBoundedContext, TModel>)))
                .ToArray()
                .PipeTo(asserts =>
                    new AggregateResultException<TBoundedContext, TModel>(businessOperationData, asserts));
            if (invalid.Result == DomainOperationResultEnum.Exception)
            {
                return invalid;
            }
            
            var exception = specifications
                .Where(f => f.GetType().GetInterfaces()
                    .Contains(typeof(IBusinessOperationValidator<TBoundedContext, TModel>)))
                .Cast<IBusinessOperationValidator<TBoundedContext, TModel>>()
                .ToArray()
                .PipeTo(asserts =>
                    new AggregateResultWithWarnings<TBoundedContext, TModel>(businessOperationData, asserts));

            if (exception.Result == DomainOperationResultEnum.WithWarnings)
            {
                return exception;
            }

            return new AggregateResultSuccess<TBoundedContext, TModel>(businessOperationData);
        }
    }
}