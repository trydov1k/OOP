namespace Incapsulation.Failures.Models;

public class Date
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }

    public Date(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public bool IsEarlierThan(Date date)
    {
        
        return Year < date.Year 
               || (Year == date.Year && Month < date.Month) 
               || (Month == date.Month && Day < date.Day);
    }
}