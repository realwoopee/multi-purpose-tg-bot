using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class CounterDao : BaseDao, ICounterDao
    {
        public CounterDao(AppConfiguration appConfig) : base(appConfig)
        {
        }

        public async Task UpdateElseCreateCounter(long chatId, long userId, long counts)
        {
            await ExecuteAsync(connection => connection.ExecuteAsync(
                """
                insert into counters(chat_id, user_id, counter)
                                  values (@chatId, @userId, @counts)
                                  on conflict (chat_id, user_id) do update
                                  set counter = counters.counter + @counts
                """,
                new { chatId, userId, counts }
            ));
        }

        public async Task<List<LeaderboardEntry>> GetCountersAndUsersWithLimit(
            long chatId,
            int limit = 10)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QueryAsync(
                    """
                    select chat_id, counters.user_id, counter, first_name, last_name, user_name 
                                          from counters, users
                                          where counters.chat_id = @chatId and counters.user_id = users.user_id
                                          order by counter desc
                                          limit @limit
                    """,
                    new { chatId, limit }
                );

                return result
                    .Select(c => new LeaderboardEntry(
                        new User
                        {
                            Id = (int)c.user_id,
                            FirstName = c.first_name,
                            LastName = c.last_name,
                            Username = c.user_name
                        },
                        new Counter(c.chat_id, c.user_id, c.counter)
                    ))
                    .ToList();
            });
        }

        public async Task<Counter> GetPersonalCounter(long chatId, long userId)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QuerySingleAsync<long>(
                    """
                    select counter from counters 
                                          where chat_id = @chatId and user_id = @userId
                    """,
                    new { chatId, userId }
                );

                return new Counter(chatId, userId, result);
            });
        }

        public async Task<long> GetTotalCount()
        {
            return await ExecuteAsync(connection =>
                connection.ExecuteScalarAsync<long>(@"select sum(counter) from counters")
            );
        }
    }
}