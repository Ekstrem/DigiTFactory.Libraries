using System.Threading;
using System.Threading.Tasks;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Specification;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Репозиторий получения единственного агрегата.
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности.</typeparam>
	public interface IRepositoryReadSingle<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="mode">Режим поиска единственного значения.</param>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		TEntity Get<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			SingleReadMode mode)
			where TIn : IQuery;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="mode">Режим поиска единственного значения.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		Task<TEntity> GetAsync<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			SingleReadMode mode = default,
			CancellationToken cancellationToken = default)
			where TIn : IQuery;
	}

	/// <summary>
	/// Репозиторий получения единственного агрегата.
	/// </summary>
	public interface IRepositoryReadSingle
	{
		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="mode">Режим поиска единственного значения.</param>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		TEntity Get<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			SingleReadMode mode)
			where TEntity : class, IEntity
			where TIn : IQuery;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="mode">Режим поиска единственного значения.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		Task<TEntity> GetAsync<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			SingleReadMode mode = default,
			CancellationToken cancellationToken = default)
			where TEntity : class, IEntity
			where TIn : IQuery;
	}
}
