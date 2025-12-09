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
            var future = diff.TotalSeconds < 0;
            var span = TimeSpan.FromSeconds(Math.Abs(diff.TotalSeconds));

            static string suffix(bool f) => f ? "from now" : "ago";

            if (span.TotalSeconds < 5)
                return future ? "in a moment" : "just now";
            if (span.TotalSeconds < 60)
                return $"{(int)span.TotalSeconds}s {suffix(future)}";
            if (span.TotalMinutes < 2)
                return $"1 min {suffix(future)}";
            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} min {suffix(future)}";
            if (span.TotalHours < 2)
                return $"1 hr {suffix(future)}";
            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hr {suffix(future)}";
            if (span.TotalDays < 2)
                return $"1 day {suffix(future)}";
            if (span.TotalDays < 7)
                return $"{(int)span.TotalDays} days {suffix(future)}";
            if (span.TotalDays < 30)
                return $"{(int)(span.TotalDays / 7)} wk {suffix(future)}";
            if (span.TotalDays < 365)
                return $"{(int)(span.TotalDays / 30)} mo {suffix(future)}";
            return $"{(int)(span.TotalDays / 365)} yr {suffix(future)}";
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
                string jumada2 = useAlThaniyahForJumada ? "جمادى الثانية" : "جمادى الآخرة";
                string[] months = ["", "محرم", "صفر", "ربيع الأول", "ربيع الثاني", "جمادى الأولى", jumada2, "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة"];
                return $"{day:00} {months[month]} {year:0000}";
            }

            return $"{day:00}/{month:00}/{year:0000}";
        }
    }
}
