using System;
using System.Linq.Expressions;
using Hive.SeedWorks.LifeCircle;
using Hive.SeedWorks.Pipelines.Abstractions;

namespace Hive.SeedWorks.Specification
{
    /// <summary>
    /// Спецификация для запросов.
    /// </summary>
    public interface IQuerySpecification<in TIn, TEntity>
        where TIn : IQuery
        where TEntity : IEntity
    {
        /// <summary>
        /// Возаращает выражение для рассчёта выволняется ли условие
        /// </summary>
        Expression<Func<TEntity, bool>> IsSatisfiedBy(TIn dto);
    }
}