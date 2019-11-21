using System;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.TacticalPatterns
{
    public abstract class AggregateBusinessOperationFactory<TBoundedContext> : 
        IAggregateBusinessOperationFactory<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IHasVersion _version;
        private readonly IBoundedContextScope<TBoundedContext> _scope;
        private readonly Guid _id;

        protected AggregateBusinessOperationFactory(
            Guid id,
            IHasVersion version,
            IBoundedContextScope<TBoundedContext> scope)
        {
            _id = id;
            _version = version;
            _scope = scope;
        }

        protected AggregateBusinessOperationFactory(
            CommandToAggregate command,
            IAggregate<TBoundedContext> aggregate)
            : this(aggregate.Id, aggregate, aggregate)
        {
        }

        public Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> input, CommandToAggregate command)
        {
            var result = _scope.Validators
                .Where(f => !f.ValidateModel(input))
                .Select(m => m.GetType().Name)
                .ToList();
            if (result.Any())
            {
                return Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>>
                    .Failure(new AggregateResultFailure<TBoundedContext>(
                        null, command, null, result.First()));
            }

            return _version
                .PipeTo(v => ComplexKey.Create(_id, _version.VersionNumber))
                .PipeTo(ck => Aggregate<TBoundedContext>.CreateInstance(ck, input, _scope))
                .PipeTo(a => Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>>
                    .Success(new AggregateResultSuccess<TBoundedContext>(a, command, null)));
        }
    }
}
