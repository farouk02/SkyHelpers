namespace SkyHelpers
{
    public class DateHelper
    {
        public static string RelativeAgo(DateTime dt, System.Globalization.CultureInfo? culture = null)
        {
            if (dt == DateTime.MinValue)
                return "";

            if (culture != null)
                Resource.Culture = culture;

            var now = dt.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
            var diff = now - dt;
            bool future = diff.TotalSeconds < 0;
            var span = TimeSpan.FromSeconds(Math.Abs(diff.TotalSeconds));

            string GetString(string pastKey, string futureKey, object arg = null)
            {
                string key = future ? futureKey : pastKey;
                string format = Resource.ResourceManager.GetString(key, culture ?? System.Globalization.CultureInfo.CurrentUICulture) ?? "";
                return arg == null ? format : string.Format(format, arg);
            }

            if (span.TotalSeconds < 5)
                return GetString("JustNow", "InAMoment");
            if (span.TotalSeconds < 60)
                return GetString("SecondsPast", "SecondsFuture", (int)span.TotalSeconds);
            if (span.TotalMinutes < 2)
                return GetString("MinutePast", "MinuteFuture");
            if (span.TotalMinutes < 60)
                return GetString("MinutesPast", "MinutesFuture", (int)span.TotalMinutes);
            if (span.TotalHours < 2)
                return GetString("HourPast", "HourFuture");
            if (span.TotalHours < 24)
                return GetString("HoursPast", "HoursFuture", (int)span.TotalHours);
            if (span.TotalDays < 2)
                return GetString("DayPast", "DayFuture");
            if (span.TotalDays < 7)
                return GetString("DaysPast", "DaysFuture", (int)span.TotalDays);
            if (span.TotalDays < 30)
                return GetString("WeeksPast", "WeeksFuture", (int)(span.TotalDays / 7));
            if (span.TotalDays < 365)
                return GetString("MonthsPast", "MonthsFuture", (int)(span.TotalDays / 30));
            return GetString("YearsPast", "YearsFuture", (int)(span.TotalDays / 365));
        }

        public static string GetHijriDate(DateTime dt, int adjustDays = 0, bool showMonthNames = true, bool useAlThaniyahForJumada = true, System.Globalization.CultureInfo? culture = null)
        {
            dt = dt.AddDays(adjustDays);
            var hijri = new System.Globalization.HijriCalendar();
            int day = hijri.GetDayOfMonth(dt);
            int month = hijri.GetMonth(dt);
            int year = hijri.GetYear(dt);
            
            if (culture != null)
                Resource.Culture = culture;
             
            // Explicitly use the culture for resource lookup if provided, otherwise default fallback.
            var cultureInfo = culture ?? System.Globalization.CultureInfo.CurrentUICulture;

            if (showMonthNames)
            {
                string jumada2 = Resource.ResourceManager.GetString(useAlThaniyahForJumada ? "Jumada2_AlThaniyah" : "Jumada2_AlAkherah", cultureInfo) ?? "";
                
                // Fetch months from resources
                string[] months = new string[13];
                months[0] = "";
                for (int i = 1; i <= 12; i++)
                {
                   if (i == 6) 
                       months[i] = jumada2;
                   else
                       months[i] = Resource.ResourceManager.GetString($"HijriMonth{i}", cultureInfo) ?? "";
                }

                return $"{day:00} {months[month]} {year:0000}";
            }

            return $"{day:00}/{month:00}/{year:0000}";
        }
    }
}
