namespace FirstProject___Test.Helpful
{

    public static class TimeHelper
    {
        public static string TimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalDays >= 1)
            {
                int days = (int)timeSpan.TotalDays;
                return $"{days} {(days == 1 ? "day" : "days")} ago";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                int hours = (int)timeSpan.TotalHours;
                return $"{hours} {(hours == 1 ? "hour" : "hours")} ago";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                int minutes = (int)timeSpan.TotalMinutes;
                return $"{minutes} {(minutes == 1 ? "minute" : "minutes")} ago";
            }
            else
            {
                return "just now";
            }
        }
    }
}
