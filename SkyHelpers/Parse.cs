namespace SkyHelpers
{
    public class Parse
    {
        public static int? Int(object? value) => int.TryParse(value?.ToString(), out int result) ? result : null;
        public static decimal Decimal(object? value) => decimal.TryParse(value?.ToString(), out decimal result) ? result : 0;
        public static double Double(object? value) => double.TryParse(value?.ToString(), out double result) ? result : 0;
        public static bool Boolean(object? value) => bool.TryParse(value?.ToString(), out bool result) && result;
        public static DateTime? DateTime(object? value) => System.DateTime.TryParse(value?.ToString(), out DateTime result) ? result : null;
    }
}
