using System.Threading.Tasks;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.Pipelines.Abstractions.Handlers
{
    /// <summary>
    /// Обработчик.
    /// </summary>
    /// <typeparam name="TIn">Входящие тип.</typeparam>
    /// <typeparam name="TOut">Исходящий тип.</typeparam>
    public interface IHandler<in TIn, out TOut>
    {
        /// <summary>
        /// Обработка.
        /// </summary>
        /// <param name="input">Входящее значение.</param>
        /// <returns>Исходящее значение.</returns>
        TOut Handle(TIn input);
    }

    /// <summary>
    /// Обработчик.
    /// </summary>
    /// <typeparam name="TIn">Входящие тип.</typeparam>
    /// <typeparam name="TOut">Исходящий тип.</typeparam>
    public interface IHandlerAsync<in TIn, TOut>
    {
        /// <summary>
        /// Обработка.
        /// </summary>
        /// <param name="input">Входящее значение.</param>
        /// <returns>Исходящее значение.</returns>
        Task<TOut> Handle(TIn input);
    }

    public interface  IHandler<in TIn, TSuccess, TFailure>
		: IHandler<TIn, Task<Result<TSuccess, TFailure>>> { }
}
