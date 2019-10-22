using System;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.LifeCircle;
using Hive.SeedWorks.Pipelines.Abstractions;

namespace Hive.SeedWorks.Specification
{
    public static class SpecificationExtention
    {
        /// <summary>
        /// Преобразование спецификации к предикату.
        /// Рекоментудется кэшировать данную операцию!
        /// </summary>
        /// <param name="specification">Спецификация.</param>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Предикат для проверки объекта по спецификации в памяти.</returns>
        public static Func<TEntity, bool> ToFunc<TEntity>(
            this ISpecification<TEntity> specification)
            where TEntity : IEntity
            => specification.IsSatisfiedBy().Compile();

        /// <summary>
        /// Преобразование спецификации к предикату.
        /// Рекоментудется кэшировать данную операцию!
        /// </summary>
        /// <param name="specification">Спецификация запроса.</param>
        /// <param name="dto">Параметры спецификации.</param>
        /// <typeparam name="TIn">Тип класса параметров спецификации.</typeparam>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Предикат для проверки объекта по спецификации в памяти.</returns>
        public static Func<TEntity, bool> ToFunc<TIn, TEntity>(
            this IQuerySpecification<TIn, TEntity> specification, TIn dto)
            where TIn : IQuery
            where TEntity : IEntity
            => specification.IsSatisfiedBy(dto).Compile();

        /// <summary>
        /// Переопределение условия выборки по спецификации.
        /// </summary>
        /// <param name="enumerable">Исходное перечислениее.</param>
        /// <param name="specification">Спецификация запроса.</param>
        /// <param name="dto">Параметры спецификации.</param>
        /// <typeparam name="TIn">Тип класса параметров спецификации.</typeparam>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Отфильтрованное по спецификации перечисление.</returns>
        public static IEnumerable<TEntity> Where<TIn, TEntity>(
            this IEnumerable<TEntity> enumerable,
            IQuerySpecification<TIn, TEntity> specification, TIn dto)
            where TIn : IQuery
            where TEntity : IEntity
            => enumerable.Where(specification.ToFunc(dto));

        /// <summary>
        /// Переопределение условия выборки по спецификации.
        /// </summary>
        /// <param name="queryable">Исходный запрос.</param>
        /// <param name="specification">Спецификация запроса.</param>
        /// <param name="dto">Параметры спецификации.</param>
        /// <typeparam name="TIn">Тип класса параметров спецификации.</typeparam>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Отфильтрованный по спецификации запрос.</returns>
        public static IQueryable<TEntity> Where<TIn, TEntity>(
            this IQueryable<TEntity> queryable,
            IQuerySpecification<TIn, TEntity> specification, TIn dto)
            where TIn : IQuery
            where TEntity : IEntity
            => queryable.Where(specification.IsSatisfiedBy(dto));

        /// <summary>
        /// Переопределение условия выборки по спецификации.
        /// </summary>
        /// <param name="queryable">Исходный запрос.</param>
        /// <param name="specification">Спецификация запроса.</param>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Отфильтрованный по спецификации запрос.</returns>
        public static IQueryable<TEntity> Where<TEntity>(
            this IQueryable<TEntity> queryable,
            ISpecification<TEntity> specification)
            where TEntity : IEntity
            => queryable.Where(specification.IsSatisfiedBy());

        /// <summary>
        /// Применяет предикат к конкретному объекту.
        /// </summary>
        /// <param name="specification">Прекат над типом.</param>
        /// <param name="entity">Объект, проверяющийся на удолитворение условия.</param>
        /// <typeparam name="TEntity">Тип объекта проверяемого по спецификации.</typeparam>
        /// <returns>Результат проверки объекта на условие предеката.</returns>
        public static bool ApplyTo<TEntity>(this Func<TEntity, bool> specification, TEntity entity)
            where TEntity : IEntity
            => specification.Invoke(entity);
    }
}
