using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Common.Services;

public class LeaderboardService
{
    private readonly IUserDao _userDao;
    private readonly ICounterDao _counterDao;
    private readonly ICounterDatedDao _counterDatedDao;

    public LeaderboardService(
        IUserDao userDao,
        ICounterDao counterDao,
        ICounterDatedDao counterDatedDao)
    {
        _userDao = userDao;
        _counterDao = counterDao;
        _counterDatedDao = counterDatedDao;
    }

    public async Task<List<LeaderboardRow>> GetAllTimeLeaderboard(long chatId, int limit)
    {
        var entries = await _counterDao.GetCountersAndUsersWithLimit(chatId, limit);
        
        return entries
            .Select(e => new LeaderboardRow(
                UserFormatter.FormatUserName(e.User),
                e.WordCount))
            .ToList();
    }

    public async Task<List<LeaderboardRow>> GetDailyLeaderboard(long chatId, DateTime date, int limit)
    {
        var counters = await _counterDatedDao.GetCounters(chatId, date, limit);
        return await MapCountersToLeaderboard(counters);
    }

    public async Task<List<LeaderboardRow>> GetWeeklyLeaderboard(long chatId, DateTime endDate, int limit)
    {
        var counters = await _counterDatedDao.GetCounters(
            chatId,
            DateRange.WeekEndingOn(endDate),
            limit);

        var aggregated = counters
            .GroupBy(c => c.UserId)
            .Select(g => new { UserId = g.Key, Total = g.Sum(c => c.Value) })
            .OrderByDescending(x => x.Total);

        var rows = new List<LeaderboardRow>();
        foreach (var item in aggregated)
        {
            var user = await _userDao.GetUserById(item.UserId);
            rows.Add(new LeaderboardRow(
                UserFormatter.FormatUserName(user),
                item.Total));
        }

        return rows;
    }

    private async Task<List<LeaderboardRow>> MapCountersToLeaderboard(
        IEnumerable<CounterDated> counters)
    {
        var rows = new List<LeaderboardRow>();
        
        foreach (var counter in counters)
        {
            var user = await _userDao.GetUserById(counter.UserId);
            rows.Add(new LeaderboardRow(
                UserFormatter.FormatUserName(user),
                counter.Value));
        }

        return rows;
    }
}

public record LeaderboardRow(string UserName, long WordCount);