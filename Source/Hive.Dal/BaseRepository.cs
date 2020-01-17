using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hive.Dal
{
	/// <summary>
	/// Базовый репозиторий.
	/// </summary>
	public class BaseRepository
	{
		/// <summary>
		/// Контекст для доступа к БД.
		/// </summary>
		protected readonly DbContext _context;
		/// <summary>
		/// Кэш DbSet'ов.
		/// </summary>
		private readonly Dictionary<Type, object> _sets;

		/// <summary>
		/// Конструктор репозитория.
		/// </summary>
		/// <param name="context">Контекст подключения к базе данных.</param>
		public BaseRepository(DbContext context)
		{
			_context = context;
			_sets = new Dictionary<Type, object>();
		}

		/// <summary>
		/// Представляет коллекцию всех сущностей указанного типа, которые
		/// содержатся в контексте или могут быть запрошены из БД.
		/// </summary>
		/// <typeparam name="TModel">Модель сущности.</typeparam>
		protected DbSet<TModel> GetSet<TModel>()
			where TModel : class
		{
			var type = typeof(TModel);
			if (!_sets.ContainsKey(type))
			{
				_sets.Add(type, _context.Set<TModel>());
			}
			return (DbSet<TModel>)_sets[type];
		}
	}
}
