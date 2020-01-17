using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal.RawSql
{
    public class SqlProgrammability : ISqlProgrammability
    {
        private readonly DatabaseFacade _database;

        private readonly DbConnection _dbConnection;

        public SqlProgrammability(DbContext dbContext)
        {
            _database = dbContext.Database;
            _dbConnection = dbContext.Database.GetDbConnection();
        }

        /// <inheritdoc />
        public async Task<TResult> ExecScalarFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters)
        {
            var sql = $"SELECT {BuildSqlFunc(sqlFuncName, parameters)}";
            var result = await ExecuteCommand(sql, parameters, cmd => cmd.ExecuteScalarAsync());
            return (TResult) result;
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<TResult>> ExecTableFunc<TResult>(string sqlFuncName, params IDbDataParameter[] parameters)
            where TResult : class
        {
            var sql = $"SELECT * FROM {BuildSqlFunc(sqlFuncName, parameters)}";
            return ExecQueryList<TResult>(sql, parameters);
        }

        /// <inheritdoc />
        public Task ExecStoredProc(string sqlProcName, params IDbDataParameter[] parameters)
        {
            var sql = BuildSqlStoredProc(sqlProcName, parameters);
            return ExecuteCommand(sql, parameters, cmd => cmd.ExecuteNonQueryAsync());
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<TResult>> ExecStoredProcList<TResult>(string sqlProcName, params IDbDataParameter[] parameters)
            where TResult : class
        {
            var sql = BuildSqlStoredProc(sqlProcName, parameters);
            return ExecQueryList<TResult>(sql, parameters);
        }

        /// <inheritdoc />
        public async Task<TResult> ExecStoredProcSingle<TResult>(string sqlProcName, params IDbDataParameter[] parameters)
            where TResult : class
        {
            var sql = BuildSqlStoredProc(sqlProcName, parameters);
            using (var reader = await ExecuteCommand(sql, parameters, cmd => cmd.ExecuteReaderAsync()))
            {
                var mapper = new SqlResultMapper<TResult>();
                return mapper.MapSingle(reader);
            }
        }

        private async Task<IReadOnlyList<TResult>> ExecQueryList<TResult>(string sqlQuery, params IDbDataParameter[] parameters)
            where TResult : class
        {
            using (var reader = await ExecuteCommand(sqlQuery, parameters, cmd => cmd.ExecuteReaderAsync()))
            {
                var mapper = new SqlResultMapper<TResult>();
                return mapper.MapList(reader);
            }
        }

        private TResult ExecuteCommand<TResult>(
            string sql,
            IDbDataParameter[] parameters,
            Func<DbCommand, TResult> funcLoad)
        {
            OpenConnectionIfNeeds();

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.Transaction = _database.CurrentTransaction?.GetDbTransaction();
				dbCommand.CommandTimeout = 60;

                if (parameters != null)
                {
                    foreach (var pr in parameters)
                    {
                        dbCommand.Parameters.Add(pr);
                    }
                }

                return funcLoad(dbCommand);
            }
        }

        private void OpenConnectionIfNeeds()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
        }

        private string BuildSqlStoredProc(string sqlStoredProcName, IDbDataParameter[] parameters)
        {
            return $"EXEC {sqlStoredProcName} {string.Join(",", parameters.Select(ToString))};";
        }

        private string BuildSqlFunc(string sqlFuncName, IDbDataParameter[] parameters)
        {
            return $"{sqlFuncName}({string.Join(",", parameters.Select(ToString))});";
        }

        private string ToString(IDbDataParameter parameter)
        {
            return parameter.Direction == ParameterDirection.Output
                ? $"{parameter.ParameterName} out"
                : parameter.ParameterName;
        }
    }
}