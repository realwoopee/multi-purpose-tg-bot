using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Telegram.Bot.Types;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class UserDao : IUserDao
    {
        private readonly NpgsqlConnection _connection;
        public UserDao(AppConfiguration appConfig)
        {
            _connection = new NpgsqlConnection(appConfig.DbConnectionString);
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                await _connection.OpenAsync();
                await _connection.QueryAsync(
                    $@"insert into users(user_id, first_name, last_name, user_name)
                       values (@user_id, @first_name, @last_name, @user_name)
                       on conflict (user_id) do update
                            set first_name = @first_name,
                            last_name = @last_name,
                            user_name = @user_name;",
                    new
                    {
                        user_id = user.Id,
                        first_name = user.FirstName,
                        last_name = user.LastName,
                        user_name = user.Username
                    });
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<User> GetUserById(long userId)
        {
            try
            {
                await _connection.OpenAsync();
                var result = await _connection.QuerySingleAsync<User>(
                    $@"select * from users where user_id = @user_id",
                    new { user_id = userId });
                return result;
            }
            finally
            {
                await _connection.CloseAsync();
            }

        }
    }
}