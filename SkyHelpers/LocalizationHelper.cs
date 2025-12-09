using System.Globalization;
using SkyHelpers.Resources;

namespace SkyHelpers
{
    internal static class LocalizationHelper
    {
        public static string GetString(string key, CultureInfo? culture = null)
        {
            return Resource.ResourceManager.GetString(key, culture ?? CultureInfo.CurrentUICulture) ?? "";
        }

        public static string GetString(string key, CultureInfo? culture, params object[] args)
        {
            string format = GetString(key, culture ?? CultureInfo.CurrentUICulture);
            return args.Length > 0 ? string.Format(format, args) : format;
        }

        public static string GetRelativeString(bool isFuture, string pastKey, string futureKey, CultureInfo? culture = null, object? arg = null)
        {
            string key = isFuture ? futureKey : pastKey;
            return arg == null ? GetString(key, culture) : GetString(key, culture, arg);
        }
    }
}
