using System;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Monads;

namespace DigiTFactory.Libraries.SeedWorks.Result
{
    /// <summary>
    /// ����� �������� ��� ������ ��� �������� ���������.
    /// </summary>
    public static class TaskResult
    {
        /// <summary>
        /// ���������� � ��������� ���� ���������� ��������� ��������.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSuccess"></typeparam>
        /// <returns></returns>
        public static Task<TResult> Match<TResult, TSuccess>(
            this Task<TSuccess> task,
            Func<TSuccess, TResult> success,
            Func<AggregateException, TResult> failure)
            => Match(task, success, failure, () => default);


        /// <summary>
        /// ���������� � ��������� ���� ���������� ��������� ��������.
        /// </summary>
        /// <param name="task">������ �������.</param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <param name="cancel"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSuccess"></typeparam>
        /// <returns></returns>
        public static async Task<TResult> Match<TResult, TSuccess>(
            this Task<TSuccess> task,
            Func<TSuccess, TResult> success,
            Func<AggregateException, TResult> failure,
            Func<TResult> cancel)
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                return success(result);
            }
            catch (OperationCanceledException)
            {
                return cancel();
            }
            catch (AggregateException ex)
            {
                return failure(ex);
            }
            catch (Exception ex)
            {
                return failure(new AggregateException(ex));
            }
        }

        /// <summary>
        /// �������� ������ TPL � Result-������, � Exception ������������� �����������.
        /// </summary>
        /// <param name="task">������ ������.</param>
        /// <typeparam name="TSuccess">�������� ���������.</typeparam>
        /// <returns>Result-������.</returns>
        /// <exception cref="ApplicationException"></exception>
        public static async Task<Result<TSuccess, Exception>> ToResult<TSuccess>(this Task<TSuccess> task)
        {
            try
            {
                await task;
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        return Result<TSuccess, Exception>.Success(task.Result);
                    case TaskStatus.Faulted:
                        return Result<TSuccess, Exception>.Failure(task.Exception);
                    case TaskStatus.Canceled:
                        return null;
                    default: throw new ApplicationException();
                }
            }
            catch (Exception e)
            {
                return Result<TSuccess, Exception>.Failure(e);
            }
        }

        /// <summary>
        /// �������������� Result-������ � ������ TPL.
        /// </summary>
        /// <param name="resultMonad">Result-������.</param>
        /// <typeparam name="TSuccess">�������� ���������.</typeparam>
        /// <returns>������ ������.</returns>
        public static Task<TSuccess> ToTask<TSuccess>(this Result<TSuccess, Exception> resultMonad)
            => resultMonad.Match(Task.FromResult, Task.FromException<TSuccess>);

        /// <summary>
        /// �������������� Result-������ � ������ TPL.
        /// </summary>
        /// <param name="resultMonadTask">Result-������.</param>
        /// <typeparam name="TSuccess">�������� ���������.</typeparam>
        /// <returns>������ ������.</returns>
        public static async Task<TSuccess> ToTask<TSuccess>(this Task<Result<TSuccess, Exception>> resultMonadTask)
        {
            var result = await resultMonadTask.ConfigureAwait(false);
            return await result.ToTask().ConfigureAwait(false);
        }

    }
}