namespace SkyHelpers
{
    public class DateHelper
    {
        public static string RelativeAgo(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return "";

            var now = dt.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
            var diff = now - dt;
            bool future = diff.TotalSeconds < 0;
            var span = TimeSpan.FromSeconds(Math.Abs(diff.TotalSeconds));

            if (span.TotalSeconds < 5)
                return LocalizationHelper.GetRelativeString(future, "JustNow", "InAMoment");
            if (span.TotalSeconds < 60)
                return LocalizationHelper.GetRelativeString(future, "SecondsPast", "SecondsFuture", (int)span.TotalSeconds);
            if (span.TotalMinutes < 2)
                return LocalizationHelper.GetRelativeString(future, "MinutePast", "MinuteFuture");
            if (span.TotalMinutes < 60)
                return LocalizationHelper.GetRelativeString(future, "MinutesPast", "MinutesFuture", (int)span.TotalMinutes);
            if (span.TotalHours < 2)
                return LocalizationHelper.GetRelativeString(future, "HourPast", "HourFuture");
            if (span.TotalHours < 24)
                return LocalizationHelper.GetRelativeString(future, "HoursPast", "HoursFuture", (int)span.TotalHours);
            if (span.TotalDays < 2)
                return LocalizationHelper.GetRelativeString(future, "DayPast", "DayFuture");
            if (span.TotalDays < 7)
                return LocalizationHelper.GetRelativeString(future, "DaysPast", "DaysFuture", (int)span.TotalDays);
            if (span.TotalDays < 30)
                return LocalizationHelper.GetRelativeString(future, "WeeksPast", "WeeksFuture", (int)(span.TotalDays / 7));
            if (span.TotalDays < 365)
                return LocalizationHelper.GetRelativeString(future, "MonthsPast", "MonthsFuture", (int)(span.TotalDays / 30));
            return LocalizationHelper.GetRelativeString(future, "YearsPast", "YearsFuture", (int)(span.TotalDays / 365));
        }

        public static string GetHijriDate(DateTime dt, int adjustDays = 0, bool showMonthNames = true, bool useAlThaniyahForJumada = true)
        {
            dt = dt.AddDays(adjustDays);
            var hijri = new System.Globalization.HijriCalendar();
            int day = hijri.GetDayOfMonth(dt);
            int month = hijri.GetMonth(dt);
            int year = hijri.GetYear(dt);

            if (showMonthNames)
            {
                string jumada2 = LocalizationHelper.GetString(useAlThaniyahForJumada ? "Jumada2_AlThaniyah" : "Jumada2_AlAkherah");

                // Fetch months from resources
                string[] months = new string[13];
                months[0] = "";
                for (int i = 1; i <= 12; i++)
                {
                    if (i == 6)
                        months[i] = jumada2;
                    else
                        months[i] = LocalizationHelper.GetString($"HijriMonth{i}");
                }

                return $"{day:00} {months[month]} {year:0000}";
            }

            return $"{day:00}/{month:00}/{year:0000}";
        }
    }
}
