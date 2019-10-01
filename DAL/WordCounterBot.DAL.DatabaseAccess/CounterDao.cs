using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class CounterDao : ICounterDao
    {
        private NpgsqlConnection _connection;

        public CounterDao(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration["DB_CONNECTION_STRING"]);
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
