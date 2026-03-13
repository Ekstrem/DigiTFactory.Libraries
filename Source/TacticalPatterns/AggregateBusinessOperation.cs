using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Базовый класс бизнес-операции над агрегатом.
    /// </summary>
    /// <typeparam name="TSelf">Тип наследника (CRTP).</typeparam>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public abstract class AggregateBusinessOperation<TSelf, TBoundedContext> : IAggregateBusinessOperation<TBoundedContext>
        where TSelf : AggregateBusinessOperation<TSelf, TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IBusinessOperationValidator<TBoundedContext> _validator;

        /// <summary>
        /// Создает экземпляр бизнес-операции с необязательным валидатором.
        /// </summary>
        /// <param name="validator">Валидатор бизнес-операции (может быть null).</param>
        protected AggregateBusinessOperation(IBusinessOperationValidator<TBoundedContext> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Выполнить бизнес-операцию.
        /// </summary>
        /// <param name="model">Анемичная модель.</param>
        /// <param name="command">Команда к агрегату.</param>
        /// <param name="scope">Область ограниченного контекста.</param>
        /// <returns>Результат выполнения бизнес-операции.</returns>
        public AggregateResult<TBoundedContext, IAnemicModel<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> model,
            CommandToAggregate command,
            IBoundedContextScope<TBoundedContext> scope)
        {
            if (_validator != null && !_validator.ValidateModel(model))
            {
                var failedData = BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>>
                    .Commit<TBoundedContext, IAnemicModel<TBoundedContext>>(model, model);

                return new AggregateResultException<TBoundedContext, IAnemicModel<TBoundedContext>>(failedData);
            }

            var operationData = BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>>
                .Commit<TBoundedContext, IAnemicModel<TBoundedContext>>(model, model);

            return new AggregateResultSuccess<TBoundedContext, IAnemicModel<TBoundedContext>>(operationData);
        }
    }
}
