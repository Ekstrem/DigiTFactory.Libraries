using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Reactive;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.TacticalPatterns
{
    public class Notifier<TBoundedContext, TAggregate> :
        DomainEventNotifier<TBoundedContext, TAggregate>,
        IAggregate<TBoundedContext>,
        IObservable<AggregateResultDiff<TBoundedContext, TAggregate>>
        where TAggregate  : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly List<IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>> _observers;
        private IHasComplexKey _key;
        private ICommandMetadata _previousOperation;

        protected Notifier(
            TAggregate aggregate,
            List<IObserver<AggregateResultDiff<TBoundedContext,TAggregate>>> observers)
            : base(aggregate)
        {
            _observers = observers;
        }

        public Guid Id => _key.Id;

        public long Version => _key.Version;

        //public ICommandMetadata PreviousOperation => Aggregate.PreviousOperation;

        public IDictionary<string, IValueObject> Invariants => Aggregate.Invariants;

        public Guid CorrelationToken => throw new NotImplementedException();

        public string CommandName => throw new NotImplementedException();

        public string SubjectName => throw new NotImplementedException();

        public Guid BranchId => throw new NotImplementedException();

        public IDisposable Subscribe(IObserver<AggregateResultDiff<TBoundedContext, TAggregate>> observer)
            => Unsubscriber<AggregateResultDiff<TBoundedContext, TAggregate>>.Subscribe(_observers, observer);

        private void WithObservablesDo(
            Action<IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>> action)
            => _observers.ForEach(action.Invoke);

        private void MatchAndPushNotification(AggregateResultDiff<TBoundedContext, TAggregate> result)
        {
            switch (result.Result)
            {
                case DomainOperationResult.Success:
                    WithObservablesDo(a => a.OnNext(result));
                    break;
                case DomainOperationResult.Invalid:
                    WithObservablesDo(a => a.OnNext(result));
                    break;
                case DomainOperationResult.Exception:
                    _observers.ForEach(
                        a => result
                            .PipeTo(r => new InvalidOperationException(r.Reason))
                            .Do(a.OnError));
                    break;
            }
        }
        
        public static Notifier<TBoundedContext, TAggregate> Create(
            TAggregate aggregate,
            List<IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>> observers)
            => new Notifier<TBoundedContext, TAggregate>(aggregate, observers);
    }
}