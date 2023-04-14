namespace Velocity.Domain;

public static class DateTimeExtensions
{
    public static DateTime FirstDateOfWeek(this DateTime dateTime)
    {
        int diff = (7 + (dateTime.Date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dateTime.AddDays(-1 * diff).Date;
    }
}