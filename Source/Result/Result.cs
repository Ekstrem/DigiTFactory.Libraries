using System;
using System.Threading.Tasks;

namespace Hive.SeedWorks.Result
{
	/// <summary>
	/// Результат обработки.
	/// </summary>
	/// <typeparam name="TSuccess">Тип удачной операции.</typeparam>
	/// <typeparam name="TFailure">Тип неудачной операции.</typeparam>
	public class Result<TSuccess, TFailure>
	{
		private readonly TFailure _failure;
		private readonly TSuccess _success;
		private readonly bool _isSuccess;

		/// <summary>
		/// Конструктор для удачной операции.
		/// </summary>
		/// <param name="success">Удачный результат операции.</param>
		private Result(TSuccess success)
		{
			_success = success;
			_isSuccess = true;
		}

		/// <summary>
		/// Конструктор для неудачной операции.
		/// </summary>
		/// <param name="failure">Неудачный результат операции.</param>
		private Result(TFailure failure)
		{
			_failure = failure;
			_isSuccess = false;
		}

		public TResult Match<TResult>(
			Func<TSuccess, TResult> success,
			Func<TFailure, TResult> failure)
			=> _isSuccess
				? success(_success)
				: failure(_failure);

		public bool IsSuccess => _isSuccess;

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"is Success: {_isSuccess}";

        /// <summary>
		/// Фабричный метод удачной операции.
		/// </summary>
		/// <param name="success">Результат удачной операции.</param>
		/// <returns>Результирующая развилка.</returns>
		public static Result<TSuccess, TFailure> Success(TSuccess success)
			=> new Result<TSuccess, TFailure>(success);

		/// <summary>
		/// Фабричный метод удачной операции.
		/// </summary>
		/// <param name="failure">Результат неудачной операции.</param>
		/// <returns>Результирующая развилка.</returns>
		public static Result<TSuccess, TFailure> Failure(TFailure failure)
			=> new Result<TSuccess, TFailure>(failure);

		public static implicit operator Task<Result<TSuccess, TFailure>>(Result<TSuccess, TFailure> result)
			=> Task.FromResult(result);

		public static implicit operator TSuccess(Result<TSuccess, TFailure> result)
		{
			if (result.IsSuccess)
				return result._success;
			throw new InvalidOperationException("Failure operation.");
		}
	}
}
