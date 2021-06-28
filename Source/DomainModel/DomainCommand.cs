using System.Linq;
using Fida.Services.Cfs.DomainModel.Result;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.DomainModel
{
    public abstract class DomainCommand<TBoundedContext, TAggregate> :
        IDomainCommand<TBoundedContext, TAggregate>
    where TAggregate : IAggregate<TBoundedContext>
    where TBoundedContext: IBoundedContext
    {
        private readonly IAnemicModel<TBoundedContext> _anemicModel;
        private readonly string _commandName;
        private readonly IBusinessOperationValidator<TBoundedContext>[] _validators;
        private readonly IBusinessOperationAssertation<TBoundedContext>[] _assertations;

        protected DomainCommand(
            IAnemicModel<TBoundedContext> anemicModel,
            string commandName,
            IBusinessOperationValidator<TBoundedContext>[] validators,
            IBusinessOperationAssertation<TBoundedContext>[] assertations)
        {
            _anemicModel = anemicModel;
            _commandName = commandName;
            _validators = validators;
            _assertations = assertations;
        }

        /// <summary>
        /// Имя метода агрегата.
        /// </summary>
        public string CommandName => _commandName;

        public AggregateResultDiff Handle(IAnemicModel<TBoundedContext> anemicModel)
        {
            var bod = BusinessOperationData.Commit(_anemicModel, anemicModel, anemicModel.PreviousOperation);
            var invalidResult = new InvalidAggregateResult<TBoundedContext, TAggregate>(bod, _validators);
            if (invalidResult.Result == DomainOperationResult.Invalid)
            {
                return invalidResult;
            }

            var assertResult = new ExceptionAggregateResult(bod, _assertations);
            if (assertResult.Result == DomainOperationResult.Exception)
            {
                return assertResult;
            }

            return new SuccessAggregateResult(bod);
        }
    }
}