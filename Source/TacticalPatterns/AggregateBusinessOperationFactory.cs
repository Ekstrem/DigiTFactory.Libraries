using System;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
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

        public AggregateResult<TBoundedContext> Handle(
            IAnemicModel<TBoundedContext> input, CommandToAggregate command)
            => _id
                .Either(c => _id == default, s => command.CorrelationToken, n => _id)
                .PipeTo(id => ComplexKey.Create(id, command))
                .PipeTo(ck => Aggregate<TBoundedContext>.CreateInstance(ck, input, _scope))
                // TODO: add changed values
                .PipeTo(a => new AggregateResult<TBoundedContext>(a, command, null));
    }
}
