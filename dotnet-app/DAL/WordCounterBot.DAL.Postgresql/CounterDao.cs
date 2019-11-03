using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class CounterDao : ICounterDao
    {
        private readonly string _connectionString;

        public CounterDao(AppConfiguration appConfig)
        {
            _connectionString = appConfig.DbConnectionString;
        }

        public async Task AddCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.QueryAsync($@"insert into counters(""chatId"", ""userId"", counter)
                                    values (@chatId, @userId, 0)",
                    new { chatId, userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task AddCounter(long chatId, long userId, long counts)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.QueryAsync($@"insert into counters(""chatId"", ""userId"", counter)
                                    values (@chatId, @userId, @counts)",
                                        new { chatId, userId, counts });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task IncrementCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.QueryAsync($@"update counters
                                    set counter = counter + 1
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                    new { chatId, userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task IncrementCounter(long chatId, long userId, long counts)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.QueryAsync($@"update counters
                                    set counter = counter + @counts
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                        new { chatId, userId, counts });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task ResetCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.QueryAsync($@"update counters
                                    set counter = 0
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                            new { chatId, userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<long> GetCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleAsync<long>($@"select counter from counters
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                    new { chatId, userId });
                return result;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<(long userId, long counter)>> GetCounters(long chatId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<(long, long)>($@"select ""userId"", counter from counters
                                     where counters.""chatId"" = @chatId
                                     order by counter desc",
                    new { chatId });
                return result.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<(long userId, long counter)>> GetCountersWithLimit(long chatId, int limit = 10)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<(long, long)>($@"select ""userId"", counter from counters
                                     where counters.""chatId"" = @chatId
                                     order by counter desc
                                     limit @limit",
                    new { chatId, limit });

                return result.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> CheckCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<bool>($@"select exists(
                                     select 1 from counters
                                     where counters.""chatId"" = @chatId and counters.""userId"" = @userId)",
                    new { chatId, userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
