namespace WordCounterBot.Common.Entities;

public record LeaderboardEntry(User User, Counter Counter)
{
    public long WordCount => Counter.Value;
}