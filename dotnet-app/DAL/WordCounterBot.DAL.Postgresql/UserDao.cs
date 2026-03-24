using System.Threading.Tasks;
using Dapper;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Postgresql
{
    public class UserDaoPostgreSQL : BaseDao, IUserDao
    {
        public UserDaoPostgreSQL(AppConfiguration appConfig) : base(appConfig)
        {
        }

        public async Task UpdateUser(User user)
        {
            await ExecuteAsync(async connection =>
            {
                await connection.ExecuteAsync(
                    """
                    insert into users(user_id, user_name, first_name, last_name)
                                      values (@user_id, @user_name, @first_name, @last_name)
                                      on conflict (user_id) do update
                                           set user_name = @user_name,
                                           first_name = @first_name,
                                           last_name = @last_name;
                    """,
                    new
                    {
                        user_id = user.Id,
                        user_name = user.Username,
                        first_name = user.FirstName,
                        last_name = user.LastName,
                    }
                );
            });
        }

        public async Task<User> GetUserById(long userId)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QuerySingleOrDefaultAsync(
                    @"select * from users where user_id = @user_id",
                    new { user_id = userId }
                );
                
                return MapToUser(result);
            });
        }

        public async Task<User> GetUserByUserName(string username)
        {
            return await ExecuteAsync(async connection =>
            {
                var result = await connection.QuerySingleOrDefaultAsync(
                    @"select * from users where LOWER(user_name) = LOWER(@user_name)",
                    new { user_name = username }
                );

                return MapToUser(result);
            });
        }

        private static User MapToUser(dynamic result)
        {
            if (result == null)
                return null;

            return new User
            {
                Id = (int)result.user_id,
                FirstName = result.first_name,
                LastName = result.last_name,
                Username = result.user_name
            };
        }
    }
}