using System.Globalization;
namespace Recept.Services
{
    //All veckologik ligger här så veckan börjar på måndag.
    //Använder ISOWeek då den alltid börjar på måndag oavsett dator.
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
        //för att slippa anropa ISOweek direkt
        public static int GetWeeksInYear(int year)
        {
            return ISOWeek.GetWeeksInYear(year);
        }
    }
}
