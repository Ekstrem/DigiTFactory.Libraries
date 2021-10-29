using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Invariants
{
    public class BusinessOperationData<TBoundedContext, TModel> : IHasValueObjects
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly TModel _aggregate;
        private readonly TModel _model;

        protected BusinessOperationData(TModel aggregate, TModel model)
        {
            _aggregate = aggregate;
            _model = model;
        }

        public TModel Aggregate => _aggregate;

        public TModel Model => _model;
        
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

        public static BusinessOperationData<TBoundedContext, TModel> Commit<TBoundedContext, TModel>(
            TModel aggregate, TModel model)
            where TModel : IAnemicModel<TBoundedContext>
            where TBoundedContext : IBoundedContext 
            => new BusinessOperationData<TBoundedContext, TModel>(aggregate, model);
    }
}