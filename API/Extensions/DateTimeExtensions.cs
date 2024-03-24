namespace API;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateTime.UtcNow.Date;
        var dateOfBirth = dob.ToDateTime(TimeOnly.MinValue).Date;

        return (int)((today - dateOfBirth).TotalDays / 365);
    }
}
