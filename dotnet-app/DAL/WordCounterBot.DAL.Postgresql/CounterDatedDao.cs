using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class CounterDatedDao : ICounterDatedDao
    {
        private readonly string _connectionString;

        public CounterDatedDao(AppConfiguration appConfig)
        {
            _connectionString = appConfig.DbConnectionString;
        }

        public async Task UpdateElseCreateCounter(long chatId, long userId, DateTime date, long counts)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync($@"insert into counters_dated(date, chat_id, user_id, counter)
                                    values (@date, @chatId, @userId, @counts)
                                    on conflict (chat_id, user_id, date) do update
                                    set counter = counters_dated.counter + @counts",
                    new { date.Date, chatId, userId, counts });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync($@"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and counters_dated.date = @date
                                 order by counter desc
                                 limit @userLimit",
                    new { chatId, date, userLimit });

                return result.Select(c => new CounterDated
                {
                    Date = c.date,
                    ChatId = c.chat_id,
                    UserId = c.user_id,
                    Value = c.counter
                }).ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<CounterDated>> GetCounters(long chatId, DateTime dateFrom, TimeSpan dateLimit, int userLimit)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync($@"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and (counters_dated.date between @dateFrom and (@dateFrom + @dateLimit))
                                 order by date desc, user_id
                                 limit extract(day from @dateLimit) * @userLimit",
                    new { chatId, dateFrom, dateLimit, userLimit });

                return result.Select(c => new CounterDated
                {
                    Date = c.date,
                    ChatId = c.chat_id,
                    UserId = c.user_id,
                    Value = c.counter
                }).ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
