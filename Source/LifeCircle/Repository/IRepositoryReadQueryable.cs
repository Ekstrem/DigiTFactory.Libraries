using System;
using System.Linq;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Specification;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	/// <typeparam name="TEntity">Тип записи.</typeparam>
	public interface IRepositoryReadQueryable<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получение записей без спецификации.
		/// </summary>
		/// <returns>Коллекция записей.</returns>
		IQueryable<TEntity> GetQueryable();

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <returns>Коллекция агрегатов.</returns>
		IQueryable<TEntity> GetQueryable(
			ISpecification<TEntity> specification,
			IPaging paging = default);

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		IQueryable<TEntity> GetQueryable<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IPaging paging = default)
			where TIn : IQuery;
	}

	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	public interface IRepositoryReadQueryable
	{
		/// <summary>
		/// Получение записей без спецификации.
		/// </summary>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <returns>Коллекция записей.</returns>
		[Obsolete]
		IQueryable<TEntity> GetQueryable<TEntity>()
			where TEntity : class, IEntity;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		IQueryable<TEntity> GetQueryable<TEntity>(
			ISpecification<TEntity> specification,
			IPaging paging = default)
			where TEntity : class, IEntity;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		IQueryable<TEntity> GetQueryable<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IPaging paging = default)
			where TEntity : class, IEntity
			where TIn : IQuery;
	}
}
