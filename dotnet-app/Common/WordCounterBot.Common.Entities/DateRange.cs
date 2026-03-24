using System;

namespace WordCounterBot.Common.Entities;

public record DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException($"StartDate ({startDate:d}) must be <= EndDate ({endDate:d})", nameof(startDate));
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public int DaysCount => (int)(EndDate - StartDate).TotalDays + 1;
    public static DateRange NDaysEndingOn(int days, DateTime endDate) 
        => new(endDate.Date.AddDays(-days + 1), endDate.Date);
    
    public static DateRange WeekEndingOn(DateTime endDate) 
        => NDaysEndingOn(7, endDate);
    
    public static DateRange MonthEndingOn(DateTime endDate) 
        => NDaysEndingOn(31, endDate);

}
