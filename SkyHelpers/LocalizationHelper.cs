using System.Globalization;
using SkyHelpers.Resources;

namespace SkyHelpers
{
    internal static class LocalizationHelper
    {
        public static string GetString(string key)
        {
            return Resource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? "";
        }

        public static string GetString(string key, params object[] args)
        {
            string format = GetString(key);
            return args.Length > 0 ? string.Format(format, args) : format;
        }

        public static string GetRelativeString(bool isFuture, string pastKey, string futureKey, object? arg = null)
        {
            string key = isFuture ? futureKey : pastKey;
            return arg == null ? GetString(key) : GetString(key, arg);
        }
    }
}
