namespace SkyHelpers
{
    internal class Parse
    {
        internal static int? Int(object value) => int.TryParse(value?.ToString(), out int result) ? result : null;
        internal static decimal Decimal(object value) => string.IsNullOrEmpty(value?.ToString()) ? 0 : Convert.ToDecimal(value);
        internal static bool Boolean(object value) => bool.TryParse(value?.ToString(), out bool result) && result;
        internal static DateTime? DateTime(object value) => System.DateTime.TryParse(value?.ToString(), out DateTime result) ? result : null;
    }
}
