using System;
using System.Linq.Expressions;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks
{
    public interface IRepository<TBoundedContext, TAnemicModel>
        where TAnemicModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        TAnemicModel GetByIdAndVersion(Guid id, long version);
        TAnemicModel GetByCorrelationToken(Guid correlationToken);
        TAnemicModel GetBySpec(Expression<Func<SlobEntry<TAnemicModel, TBoundedContext>, bool>> filter);
    }
}