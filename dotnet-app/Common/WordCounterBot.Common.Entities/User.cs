using System;

namespace WordCounterBot.Common.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public User() {}

        public User(Telegram.Bot.Types.User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }

        public string GetFullName() => $@"{this.FirstName} {this.LastName}";
    }
}
