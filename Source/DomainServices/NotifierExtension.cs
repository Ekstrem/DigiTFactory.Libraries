using System;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.TacticalPatterns
{
    public static class NotifierExtension
    {
        public static TAggregate PipeNotifier<TBoundedContext, TAggregate>(
            this TAggregate aggregate,
            List<IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>> observers)
            where TBoundedContext : IBoundedContext
            where TAggregate : IAggregate<TBoundedContext>
            => Notifier<TBoundedContext, TAggregate>.Create(aggregate, observers);
        
        public static TAggregate PipeNotifier<TBoundedContext, TAggregate>(
            this TAggregate aggregate,
            params IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>[] observers)
            where TBoundedContext : IBoundedContext
            where TAggregate : IAggregate<TBoundedContext>
            => Notifier<TBoundedContext, TAggregate>.Create(aggregate, observers.ToList());

        public static IDisposable Subscribe<TBoundedContext, TAggregate>(
            this TAggregate aggregate,
            IObserver<AggregateResultDiff<TBoundedContext, TAggregate>> observer)
            where TBoundedContext : IBoundedContext
            where TAggregate : IAggregate<TBoundedContext>
            => aggregate is Notifier<TBoundedContext, TAggregate> domainEventNotifier
                ? domainEventNotifier.Subscribe(observer)
                : null;
    }
}