namespace API.Extensions
{
    public static class DateTimeExtention
    {
        public static int CalculateAge(this DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year- date.Year;
            if (date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
