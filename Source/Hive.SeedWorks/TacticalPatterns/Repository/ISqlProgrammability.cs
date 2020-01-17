using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Hive.SeedWorks.TacticalPatterns.Repository
{
    public interface ISqlProgrammability
    {
        /// <summary>
        /// Вызов Scalar-valued function
        /// </summary>
        /// <typeparam name="TResult">
        /// Тип результата. Простой тип (ValueType or string)
        /// </typeparam>
        /// <param name="sqlFuncName">Название функции</param>
        /// <param name="parameters">Параметры функции</param>
        /// <returns><see cref="TResult"/></returns>
        Task<TResult> ExecScalarFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters);

        /// <summary>
        /// Вызов Table-valued function
        /// </summary>
        /// <typeparam name="TResult">Тип результата (коллекция сложных объектов)</typeparam>
        /// <param name="sqlFuncName">Название функции</param>
        /// <param name="parameters">Параметры функции</param>
        /// <returns>Коллекция элементов <see cref="TResult"/></returns>
        Task<IReadOnlyList<TResult>> ExecTableFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters)
            where TResult : class;

        /// <summary>
        /// Выполнить хранимую процедуру
        /// </summary>
        /// <param name="sqlProcName">Название хранимой процедуры</param>
        /// <param name="parameters">Параметры хранимой процедуры</param>
        Task ExecStoredProc(string sqlProcName, params IDbDataParameter[] parameters);

        /// <summary>
        /// Выполнить хранимую процедуру с возвратом результата
        /// NOTE: Требования к select запроса и свойствам типа результата см. <see cref="SqlResultMapper&lt;&gt;"/>
        /// </summary>
        /// <typeparam name="TResult">Тип результата (коллекция сложных объектов)</typeparam>
        /// <param name="sqlProcName">Название хранимой процедуры</param>
        /// <param name="parameters">Параметры хранимой процедуры</param>
        /// <returns>Коллекция элементов <see cref="TResult"/></returns>
        Task<IReadOnlyList<TResult>> ExecStoredProcList<TResult>(
            string sqlProcName,
            params IDbDataParameter[] parameters)
            where TResult : class;

        /// <summary>
        /// Выполнить хранимую процедуру с возвратом результата
        /// NOTE: Требования к select запроса и свойствам типа результата см. <see cref="SqlResultMapper&lt;&gt;"/>
        /// </summary>
        /// <typeparam name="TResult">Тип результата (сложный объект, единичный результат)</typeparam>
        /// <param name="sqlProcName">Название хранимой процедуры</param>
        /// <param name="parameters">Параметры хранимой процедуры</param>
        /// <returns><see cref="TResult"/></returns>
        Task<TResult> ExecStoredProcSingle<TResult>(string sqlProcName, params IDbDataParameter[] parameters)
            where TResult : class;
    }
}