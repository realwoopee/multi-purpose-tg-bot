using System;

namespace WordCounterBot.Common.Entities
{
    public record User
    {
        public long Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Username { get; init; }

        public User() { }

        public User(Telegram.Bot.Types.User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }

        public string FullName => string.IsNullOrWhiteSpace(LastName) 
            ? FirstName 
            : $"{FirstName} {LastName}";

    }
}
