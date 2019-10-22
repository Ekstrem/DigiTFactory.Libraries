namespace Hive.SeedWorks.Pipelines
{
	/// <summary>
	/// Слой работы обработчика.
	/// </summary>
	public enum HandleLevel
	{
		/// <summary>
		/// Диагностика
		/// </summary>
		Diagnostics = 1,

        /// <summary>
        /// Секция обработки ошибок
        /// </summary>
        TryCatch = 2,

		/// <summary>
		/// Логгирование данных.
		/// </summary>
		Logining = 3,

		/// <summary>
		/// Авторизация пользователя.
		/// </summary>
		Authorization = 4,

		/// <summary>
		/// Валидация входящих данных.
		/// </summary>
		Validation = 5,

		/// <summary>
		/// Кэш данных.
		/// </summary>
		Cache = 6,

		/// <summary>
		/// База данных.
		/// </summary>
		DataBase = 7,

        /// <summary>
        /// Чтение из клиента.
        /// </summary>
        Client = 8
	}
}
