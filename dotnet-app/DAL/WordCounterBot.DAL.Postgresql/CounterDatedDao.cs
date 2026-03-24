using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class CounterDatedDao : BaseDao, ICounterDatedDao
    {
        public CounterDatedDao(AppConfiguration appConfig) : base(appConfig)
        {
        }

        public async Task UpdateElseCreateCounter(
            long chatId,
            long userId,
            DateTime date,
            long counts)
        {
            await ExecuteAsync(connection => connection.ExecuteAsync(
                """
                insert into counters_dated(date, chat_id, user_id, counter)
                                  values (@date, @chatId, @userId, @counts)
                                  on conflict (chat_id, user_id, date) do update
                                  set counter = counters_dated.counter + @counts
                """,
                new { date = date.Date, chatId, userId, counts }
            ));
        }

        public async Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QueryAsync(
                    """
                    select chat_id, user_id, counter, date from counters_dated
                                          where counters_dated.chat_id = @chatId and counters_dated.date = @date
                                          order by counter desc
                                          limit @userLimit
                    """,
                    new { chatId, date, userLimit }
                );

                return result
                    .Select(c => new CounterDated(c.date, c.chat_id, c.user_id, c.counter))
                    .ToList();
            });
        }

        public async Task<List<CounterDated>> GetCounters(
            long chatId,
            DateRange dateRange,
            int userLimit)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QueryAsync(
                    """
                    select chat_id, user_id, counter, date from counters_dated
                                          where counters_dated.chat_id = @chatId 
                                            and (counters_dated.date between @startDate and @endDate)
                                          order by date desc, user_id
                                          limit @limitCount
                    """,
                    new
                    {
                        chatId,
                        startDate = dateRange.StartDate,
                        endDate = dateRange.EndDate,
                        limitCount = userLimit * dateRange.DaysCount
                    }
                );

                return result
                    .Select(c => new CounterDated(c.date, c.chat_id, c.user_id, c.counter))
                    .ToList();
            });
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateRange dateRange)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QueryAsync(
                    """
                    select chat_id, user_id, counter, date from counters_dated
                                          where counters_dated.chat_id = @chatId 
                                            and counters_dated.user_id = @userId 
                                            and (counters_dated.date between @startDate and @endDate)
                                          order by date desc, user_id
                                          limit @limitCount
                    """,
                    new
                    {
                        chatId,
                        userId,
                        startDate = dateRange.StartDate,
                        endDate = dateRange.EndDate,
                        limitCount = dateRange.DaysCount
                    }
                );

                return result
                    .Select(c => new CounterDated(c.date, c.chat_id, c.user_id, c.counter))
                    .ToList();
            });
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateTime date)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QueryAsync(
                    """
                    select chat_id, user_id, counter, date from counters_dated
                                          where counters_dated.chat_id = @chatId 
                                            and counters_dated.user_id = @userId 
                                            and counters_dated.date = @date
                                          order by counter desc
                    """,
                    new { chatId, userId, date }
                );

                return result
                    .Select(c => new CounterDated(c.date, c.chat_id, c.user_id, c.counter))
                    .ToList();
            });
        }

        public async Task<CounterDated> GetPersonalLastCounter(long chatId, long userId)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QuerySingleAsync(
                    """
                    select chat_id, user_id, counter, date from counters_dated
                                          where counters_dated.chat_id = @chatId 
                                            and counters_dated.user_id = @userId
                                          order by date desc
                                          limit 1
                    """,
                    new { chatId, userId }
                );

                return new CounterDated(result.date, result.chat_id, result.user_id, result.counter);
            });
        }
    }
}