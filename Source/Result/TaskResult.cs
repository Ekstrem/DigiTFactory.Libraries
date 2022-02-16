using System;
using System.Threading.Tasks;
using Hive.SeedWorks.Monads;

namespace Hive.SeedWorks.Result
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
        public static Task<TResult> Match<TResult, TSuccess>(
            this Task<TSuccess> task,
            Func<TSuccess, TResult> success,
            Func<AggregateException, TResult> failure,
            Func<TResult> cancel)
        {
            switch (task.Status)
            {
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                    return task.ContinueWith(t => t.Match(success, failure, cancel).Result);
                case TaskStatus.RanToCompletion:
                    return task.ContinueWith(t => success(t.Result));
                case TaskStatus.Faulted:
                    return task.ContinueWith(t => failure(t.Exception));
                case TaskStatus.Canceled:
                    return task.ContinueWith(t => cancel());
                default:
                    return default;
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
        public static Task<TSuccess> ToTask<TSuccess>(this Task<Result<TSuccess, Exception>> resultMonadTask)
            => resultMonadTask.ContinueWith(t => t.Result.ToTask().Result);

    }
}