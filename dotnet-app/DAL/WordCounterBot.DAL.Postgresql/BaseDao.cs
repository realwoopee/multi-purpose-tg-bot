using System;
using System.Threading.Tasks;
using Npgsql;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Postgresql
{
    public abstract class BaseDao
    {
        private readonly string _connectionString;

        protected BaseDao(AppConfiguration appConfig)
        {
            _connectionString = appConfig.DbConnectionString;
        }

        protected async Task<T> ExecuteAsync<T>(Func<NpgsqlConnection, Task<T>> action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return await action(connection);
        }

        protected async Task ExecuteAsync(Func<NpgsqlConnection, Task> action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await action(connection);
        }
    }
}