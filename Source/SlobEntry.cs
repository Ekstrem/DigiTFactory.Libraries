using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks
{
    public class SlobEntry<TAnemicModel, TBoundedContext> :
        IComplexKey
        where TAnemicModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private Guid _id;
        private long _version;
        private SlobEntry() { }

        public Guid Id { get; set; }
        public long Version { get; set; }
        public Guid CorrelationToken { get; set; }
        public string SubjectName { get; set; }
        public string CommandName { get; set; }
        public string ValueObjects { get; set; }

        public static SlobEntry<TAnemicModel, TBoundedContext> Create(TAnemicModel model)
            => new SlobEntry<TAnemicModel, TBoundedContext>
            {
                Id = model.Id,
                Version = model.Version,
                CorrelationToken = default,//model.CorrelationToken,
                SubjectName = default,
                CommandName = default,
                //ValueObjects = model
                //   .GetValueObjects()
                // .PipeTo(JsonConvert.SerializeObject)
            };

    }
}