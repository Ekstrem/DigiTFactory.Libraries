using System;
using System.Linq.Expressions;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Specification
{
    /// <summary>
    /// Спецификация по идентификатору.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <typeparam name="TKey">Тип ключевого поля(ей).</typeparam>
    public class GetByIdSpec<TEntity, TKey>
        : ISpecification<TEntity>
        where TEntity : IEntity<TKey>
    {
        private readonly TKey _id;

        public GetByIdSpec(TKey id) => _id = id;

        public Expression<Func<TEntity, bool>> IsSatisfiedBy()
            => GetExpression;

        private Expression<Func<TEntity, bool>> GetExpression
            => e => e.Id.Equals(_id);
    }
}