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
        private readonly string _connectionString;
        public UserDao(AppConfiguration appConfig)
        {
            _connectionString = appConfig.DbConnectionString;
        }

        public async Task UpdateUser(User user)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(
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
                await connection.CloseAsync();
            }
        }

        public async Task<User> GetUserById(long userId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync(
                    $@"select * from users where user_id = @user_id",
                    new { user_id = userId });
                if (result != null)
                {
                    return new User()
                    {
                        Id = (int)result.user_id,
                        FirstName = result.first_name,
                        LastName = result.last_name,
                        Username = result.user_name
                    };
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}