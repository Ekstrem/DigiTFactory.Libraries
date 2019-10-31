using System;
using System.Collections.Generic;
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
        private readonly IList<IAggregateBusinessOperationFactory<TBoundedContext>> _operationFactories;
        private readonly IList<IBusinessValidator<TBoundedContext>> _validators;
        private readonly Guid _id;

        protected AggregateBusinessOperationFactory(
            Guid id,
            IHasVersion version,
            IList<IAggregateBusinessOperationFactory<TBoundedContext>> operationFactories,
            IList<IBusinessValidator<TBoundedContext>> validators)
        {
            _id = id;
            _version = version;
            _operationFactories = operationFactories;
            _validators = validators;
        }

        protected AggregateBusinessOperationFactory(
            CommandToAggregate command,
            IAggregate<TBoundedContext> aggregate)
            : this(
                aggregate.Id,
                aggregate,
                aggregate.Operations,
                aggregate.Validators)
        {
        }

        public Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> input, CommandToAggregate command)
        {
            var result = _validators
                .Where(f => !f.ValidateModel(input))
                .Select(m => m.GetType().Name)
                .ToList();
            if (result.Any())
            {
                return Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>>
                    .Failure(new AggregateResultFailure<TBoundedContext>(
                        null, command, null, result.First()));
            }

            var a = Aggregate<TBoundedContext>.CreateInstance(_id, _version, input, _operationFactories, _validators);
            return Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>>
                .Success(new AggregateResultSuccess<TBoundedContext>(a, command, null));
        }
    }
}