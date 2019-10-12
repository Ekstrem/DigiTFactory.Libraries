using System;
using System.Linq.Expressions;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Specification
{
    /// <summary>
    /// Расширяющаяся спецификация.
    /// </summary>
    public interface ISpecificationEx<TEntity> :
        ISpecification<TEntity>
    {
        Expression<Func<IEntity, bool>> Or(Expression<Func<TEntity, bool>> predicate);
        Expression<Func<IEntity, bool>> And(Expression<Func<TEntity, bool>> predicate);
        Expression<Func<IEntity, bool>> Not(Expression<Func<TEntity, bool>> predicate);
    }
}