using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.TacticalPatterns
{
    public interface IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        string Name { get; }

        AggregateResult<TBoundedContext> Handle(IAnemicModel<TBoundedContext> input, CommandToAggregate command);
    }

    public abstract class AggregateBusinessOperation<TBusinessOperation, TBoundedContext>
        : IAggregateBusinessOperation<TBoundedContext>
        where TBusinessOperation : IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IBusinessOperationValidator<TBusinessOperation, TBoundedContext> _validator;

        protected AggregateBusinessOperation(
            IBusinessOperationValidator<TBusinessOperation, TBoundedContext> validator)
        {
            _validator = validator;
        }

        public virtual string Name => typeof(TBusinessOperation).Name;

        public AggregateResult<TBoundedContext> Handle(IAnemicModel<TBoundedContext> input, CommandToAggregate command)
            => input.Id
                .Either(c => c == default, s => command.CorrelationToken, n => n)
                .PipeTo(id => ComplexKey.Create(id, command))
                .PipeTo(ck => Aggregate<TBoundedContext>.CreateInstance(ck, input, _scope))
                // TODO: add changed values
                .PipeTo(a => new AggregateResult<TBoundedContext>(a, command, null));
    }
}
