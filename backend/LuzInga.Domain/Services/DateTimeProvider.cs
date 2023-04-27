namespace LuzInga.Domain.Services;

public static class DateTimeProvider
{
    private static DateTime? _dateTime = null;
    public static DateTime Now => _dateTime ?? DateTime.Now;
    public static void SetDateTime(DateTime time) => _dateTime = time;
    public static void ResetTime() => _dateTime = null;
}
