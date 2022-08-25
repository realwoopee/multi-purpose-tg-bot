using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task UpdateElseCreateCounter(
            long chatId,
            long userId,
            DateTime date,
            long counts
        )
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(
                    @"insert into counters_dated(date, chat_id, user_id, counter)
                                    values (@date, @chatId, @userId, @counts)
                                    on conflict (chat_id, user_id, date) do update
                                    set counter = counters_dated.counter + @counts",
                    new { date.Date, chatId, userId, counts }
                );
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
                var result = await connection.QueryAsync(
                    @"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and counters_dated.date = @date
                                 order by counter desc
                                 limit @userLimit",
                    new { chatId, date, userLimit }
                );

                return result
                    .Select(
                        c =>
                            new CounterDated
                            {
                                Date = c.date,
                                ChatId = c.chat_id,
                                UserId = c.user_id,
                                Value = c.counter
                            }
                    )
                    .ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<CounterDated>> GetCounters(
            long chatId,
            DateTime startDate,
            DateTime endDate,
            int userLimit
        )
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync(
                    @"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and (counters_dated.date between @startDate and @endDate)
                                 order by date desc, user_id
                                 limit (EXTRACT(epoch FROM (@endDate - @startDate))/86400::int + 1) * @userLimit",
                    new { chatId, startDate, endDate, userLimit }
                );

                return result
                    .Select(
                        c =>
                            new CounterDated
                            {
                                Date = c.date,
                                ChatId = c.chat_id,
                                UserId = c.user_id,
                                Value = c.counter
                            }
                    )
                    .ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateTime startDate,
            DateTime endDate
        )
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync(
                    @"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and counters_dated.user_id = @userId and (counters_dated.date between @startDate and @endDate)
                                 order by date desc, user_id
                                 limit (EXTRACT(epoch FROM (@endDate - @startDate))/86400::int + 1)",
                    new { chatId, userId, startDate, endDate }
                );

                return result
                    .Select(
                        c =>
                            new CounterDated
                            {
                                Date = c.date,
                                ChatId = c.chat_id,
                                UserId = c.user_id,
                                Value = c.counter
                            }
                    )
                    .ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateTime date
        )
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync(
                    @"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and counters_dated.user_id = @userId and counters_dated.date = @date
                                 order by counter desc",
                    new { chatId, userId, date }
                );

                return result
                    .Select(
                        c =>
                            new CounterDated
                            {
                                Date = c.date,
                                ChatId = c.chat_id,
                                UserId = c.user_id,
                                Value = c.counter
                            }
                    )
                    .ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<CounterDated> GetPersonalLastCounter(long chatId, long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleAsync(
                    @"select chat_id, user_id, counter, date from counters_dated
                                 where counters_dated.chat_id = @chatId and counters_dated.user_id = @userId
                                 order by date desc
                                 limit 1",
                    new { chatId, userId }
                );

                return new CounterDated
                {
                    Date = result.date,
                    ChatId = result.chat_id,
                    UserId = result.user_id,
                    Value = result.counter
                };
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
