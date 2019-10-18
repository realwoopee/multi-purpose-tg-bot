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
        private readonly NpgsqlConnection _connection;

        public CounterDao(AppConfiguration appConfig)
        {
            _connection = new NpgsqlConnection(appConfig.DbConnectionString);
        }

        public async Task AddCounter(long chatId, long userId)
        {
            await _connection.OpenAsync();
            await _connection.QueryAsync($@"insert into counters(""chatId"", ""userId"", counter)
                                    values (@chatId, @userId, 0)",
                                    new { chatId, userId });
            await _connection.CloseAsync();
        }

        public async Task AddCounter(long chatId, long userId, long counts)
        {
            await _connection.OpenAsync();
            await _connection.QueryAsync($@"insert into counters(""chatId"", ""userId"", counter)
                                    values (@chatId, @userId, @counts)",
                                    new { chatId, userId, counts });
            await _connection.CloseAsync();
        }

        public async Task IncrementCounter(long chatId, long userId)
        {
            await _connection.OpenAsync();
            await _connection.QueryAsync($@"update counters
                                    set counter = counter + 1
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                new { chatId, userId });
            await _connection.CloseAsync();
        }

        public async Task IncrementCounter(long chatId, long userId, long counts)
        {
            await _connection.OpenAsync();
            await _connection.QueryAsync($@"update counters
                                    set counter = counter + @counts
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                    new { chatId, userId, counts });
            await _connection.CloseAsync();
        }

        public async Task ResetCounter(long chatId, long userId)
        {
            await _connection.OpenAsync();
            await _connection.QueryAsync($@"update counters
                                    set counter = 0
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                        new { chatId, userId });
            await _connection.CloseAsync();
        }

        public async Task<long> GetCounter(long chatId, long userId)
        {
            await _connection.OpenAsync();
            var value = await _connection.ExecuteScalarAsync<long>($@"select counter from counters
                                 where counters.""chatId"" = @chatId and counters.""userId"" = @userId",
                                            new { chatId, userId });
            await _connection.CloseAsync();
            return value;
        }

        public async Task<List<(long userId, long counter)>> GetCountersByChat(long chatId)
        {
            //order by counter
            //limit 10
            await _connection.OpenAsync();
            var value = await _connection.QueryAsync<(long, long)>($@"select ""userId"", counter from counters
                                     where counters.""chatId"" = @chatId",
                new { chatId });
            await _connection.CloseAsync();
            return value.ToList();
        }

        public async Task<bool> CheckCounter(long chatId, long userId)
        {
            await _connection.OpenAsync();
            var value = await _connection.ExecuteScalarAsync<bool>($@"select exists(
                                     select 1 from counters
                                     where counters.""chatId"" = @chatId and counters.""userId"" = @userId)",
                                                    new { chatId, userId });
            await _connection.CloseAsync();
            return value;
        }
    }
}
