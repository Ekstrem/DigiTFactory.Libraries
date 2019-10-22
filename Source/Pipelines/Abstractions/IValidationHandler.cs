using Hive.SeedWorks.Pipelines.Abstractions.Handlers;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.Pipelines.Abstractions
{
	/// <summary>
	/// Обработчик валидации.
	/// </summary>
	/// <typeparam name="TIn">Входящий тип команды.</typeparam>
	/// <typeparam name="TFailure">Тип ошибки.</typeparam>
	public interface IValidationHandler<TIn, TFailure>
		: IHandler<TIn, Result<TIn, TFailure>> { }
}
