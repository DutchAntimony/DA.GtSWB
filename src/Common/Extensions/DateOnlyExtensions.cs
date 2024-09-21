namespace DA.GtSWB.Common.Extensions;

public static class DateOnlyExtensions
{
    public static int GetAge(this DateOnly dateOfBirth, DateTime compareTo)
    {
        var age = compareTo.Year - dateOfBirth.Year;
        if (compareTo.Month < dateOfBirth.Month ||
            compareTo.Month == dateOfBirth.Month && compareTo.Day < dateOfBirth.Day)
        {
            age--;
        }

        return age;
    }
}