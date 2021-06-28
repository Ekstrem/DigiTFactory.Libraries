using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.DomainModel
{
    public class BusinessOperationData
    {

        private readonly IAnemicModel _aggregate;
        private readonly IAnemicModel _command;
        private readonly ICommandMetadata _metadata;

        private BusinessOperationData(
            IAnemicModel aggregate,
            IAnemicModel command,
            ICommandMetadata metadata)
        {
            _aggregate = aggregate;
            _command = command;
            _metadata = metadata;
        }

        public IAnemicModel Aggregate => _aggregate;
        
        public IAnemicModel Command => _command;

        public ICommandMetadata Metadata => _metadata;

        public IDictionary<string, IValueObject> GetChangedValueObjects()
        {
            var aggregateValueObjects = _aggregate.ValueObjects;
            var dtoValueObjects = _command.ValueObjects;
            var changedValueObjects = aggregateValueObjects.Keys
                .Union(dtoValueObjects.Keys).Distinct()
                .Where(key => !Equals(
                    aggregateValueObjects.TryGetValue(key, out var bufA) ? bufA : null,
                    dtoValueObjects.TryGetValue(key, out var bufD) ? bufD : null))
                .ToDictionary(k => k, v => dtoValueObjects.ContainsKey(v) ? dtoValueObjects[v] : null);
            return changedValueObjects;
        }

        public static BusinessOperationData Commit(
            IAnemicModel aggregate,
            IAnemicModel model,
            ICommandMetadata metadata)
            => new BusinessOperationData(aggregate, model, metadata);
    }
}