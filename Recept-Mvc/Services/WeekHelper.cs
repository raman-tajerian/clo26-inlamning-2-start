using System.Globalization;
namespace Recept.Services
{
    public static class WeekHelper
    {
        public static DateOnly GetMonday(int year, int week)
        {
            DateTime monday = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
            return DateOnly.FromDateTime(monday);
        }
        public static (int year, int week) GetYearAndWeek(DateOnly date)
        {
            DateTime day = date.ToDateTime(TimeOnly.MinValue);
            return (ISOWeek.GetYear(day), ISOWeek.GetWeekOfYear(day));

        }
        public static DateOnly GetMonday(DateOnly date)
        {
            (int year, int week) = GetYearAndWeek(date);
            return GetMonday(year, week);
        }
    }
}
