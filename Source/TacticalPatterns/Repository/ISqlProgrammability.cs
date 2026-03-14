using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Вызов хранимых процедур и функций SQL.
    /// </summary>
    public interface ISqlProgrammability
    {
        /// <summary>
        /// Выполнить скалярную функцию.
        /// </summary>
        Task<TResult> ExecScalarFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters);

        /// <summary>
        /// Выполнить табличную функцию.
        /// </summary>
        Task<IReadOnlyList<TResult>> ExecTableFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters)
            where TResult : class;

        /// <summary>
        /// Выполнить хранимую процедуру.
        /// </summary>
        Task ExecStoredProc(string sqlProcName, params IDbDataParameter[] parameters);

        /// <summary>
        /// Выполнить хранимую процедуру и получить список результатов.
        /// </summary>
        Task<IReadOnlyList<TResult>> ExecStoredProcList<TResult>(string sqlProcName, params IDbDataParameter[] parameters)
            where TResult : class;

        /// <summary>
        /// Выполнить хранимую процедуру и получить единичный результат.
        /// </summary>
        Task<TResult> ExecStoredProcSingle<TResult>(string sqlProcName, params IDbDataParameter[] parameters)
            where TResult : class;
    }
}
