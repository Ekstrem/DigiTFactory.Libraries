using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.DomainModel
{
    public class DefaultAnemicModel<TBoundedContext> :
        AnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        protected DefaultAnemicModel(Guid id)
            : base(CommandToAggregate.Commit(id, default,
                default, default, default, default))
        { }

        public static DefaultAnemicModel<TBoundedContext> Create(Guid id)
            => new DefaultAnemicModel<TBoundedContext>(id);
    }
}