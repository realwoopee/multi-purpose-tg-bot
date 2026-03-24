using System;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Common.Services;

public class UserStatsService
{
    private readonly ICounterDao _counterDao;
    private readonly ICounterDatedDao _counterDatedDao;

    public UserStatsService(ICounterDao counterDao, ICounterDatedDao counterDatedDao)
    {
        _counterDao = counterDao;
        _counterDatedDao = counterDatedDao;
    }

    public async Task<UserStatistics> GetUserStatistics(long chatId, long userId, DateTime msgDate)
    {
        var totalCounter = (await _counterDao.GetPersonalCounter(chatId, userId)).Value;
        var todayCounter = (await _counterDatedDao.GetPersonalCounters(chatId, userId, msgDate))
            .Sum(c => c.Value);
        var weekCounter = (await _counterDatedDao.GetPersonalCounters(
                chatId, userId, DateRange.WeekEndingOn(msgDate)))
            .Sum(c => c.Value);
        var monthCounter = (await _counterDatedDao.GetPersonalCounters(
                chatId, userId, DateRange.MonthEndingOn(msgDate)))
            .Sum(c => c.Value);
        var lastMessageDate = (await _counterDatedDao.GetPersonalLastCounter(chatId, userId))
            .Date.Date;

        return new UserStatistics
        {
            Total = totalCounter,
            Today = todayCounter,
            Week = weekCounter,
            Month = monthCounter,
            LastMessageDate = lastMessageDate
        };
    }
}

public record UserStatistics
{
    public long Total { get; set; }
    public long Today { get; set; }
    public long Week { get; set; }
    public long Month { get; set; }
    public DateTime LastMessageDate { get; set; }
}