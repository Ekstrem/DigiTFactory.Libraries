using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Operations;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Invariants
{
    public class BusinessOperationData<TBoundedContext> :
        IHasValueObjects
        where TBoundedContext : IBoundedContext
    {
        private readonly IAnemicModel<TBoundedContext> _aggregate;
        private readonly IAnemicModel<TBoundedContext> _model;

        protected BusinessOperationData(
            IAnemicModel<TBoundedContext> aggregate,
            IAnemicModel<TBoundedContext> model)
        {
            _aggregate = aggregate;
            _model = model;
        }

        public IAnemicModel<TBoundedContext> Aggregate => _aggregate;

        public IAnemicModel<TBoundedContext> Model => _model;
        
        public IDictionary<string, IValueObject> GetValueObjects()
        {
            var aggregateValueObjects = _aggregate.GetValueObjects();
            var dtoValueObjects = Model.GetValueObjects();
            var changedValueObjects = aggregateValueObjects.Keys
                .Union(dtoValueObjects.Keys).Distinct()
                .Where(key => !Equals(
                    aggregateValueObjects.TryGetValue(key, out var bufA) ? bufA : null,
                    dtoValueObjects.TryGetValue(key, out var bufD) ? bufD : null))
                .ToDictionary(k => k, v => dtoValueObjects.ContainsKey(v) ? dtoValueObjects[v] : null);
            return changedValueObjects;
        }

        public static BusinessOperationData<TBoundedContext> Commit(
            IAnemicModel<TBoundedContext> aggregate,
            IAnemicModel<TBoundedContext> model)
            => new BusinessOperationData<TBoundedContext>(aggregate, model);
    }
}