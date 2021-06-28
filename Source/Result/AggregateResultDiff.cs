using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.DomainModel;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.Result
{
    public abstract class AggregateResultDiff
    {
        private readonly IAnemicModel _aggregate;
        private readonly ICommandMetadata _commandMetadata;
        private readonly IDictionary<string, IValueObject> _changedValueObjects;

        protected AggregateResultDiff(BusinessOperationData businessOperationData)
        {
            _aggregate = businessOperationData.Aggregate;
            _commandMetadata = businessOperationData.Metadata;
            _changedValueObjects = businessOperationData.GetChangedValueObjects();
        }

        public IAnemicModel Aggregate => _aggregate;

        public IDomainEvent Event
            => new DomainEvent(
                _aggregate.BoundedContextName, _aggregate,
                _commandMetadata, _changedValueObjects,
                Result, Reason);

        public ICommandMetadata CommandMetadata => _commandMetadata;

        public IDictionary<string, IValueObject> ChangedValueObjects => _changedValueObjects;
        
        public abstract DomainOperationResult Result { get; }
        
        public abstract string Reason { get; }
    }
}