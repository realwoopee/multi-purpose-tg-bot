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

        public async Task UpdateElseCreateCounter(long chatId, long userId, long counts)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"insert into counters(chat_id, user_id, counter)
                                    values (@chatId, @userId, @counts)
                                    on conflict (chat_id, user_id) do update
                                    set counter = counters.counter + @counts",
                    new { chatId, userId, counts });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Counter>> GetCountersWithLimit(long chatId, int limit = 10)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync(@"select chat_id, user_id, counter from counters
                                     where counters.chat_id = @chatId
                                     order by counter desc
                                     limit @limit",
                    new { chatId, limit });

                return result.Select(c => new Counter
                    {
                        ChatId = c.chat_id,
                        UserId = c.user_id,
                        Value = c.counter
                    })
                    .ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Counter> GetPersonalCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryFirstOrDefaultAsync<long?>(@"select counter from counters 
                                    where chat_id = @chatId and user_id = @userId",
                    new {chatId, userId});

                return new Counter
                {
                    ChatId = chatId,
                    UserId = userId,
                    Value = result ?? 0
                };
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<long> GetTotalCount()
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteScalarAsync<long>(@"select sum(counter) from counters");

                return result;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
