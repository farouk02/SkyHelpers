using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace SkyHelpers
{
    public static class MachineIdentifier
    {
        public static string GetUniqueIdentifier()
        {
            string identifier = GetFallbackId();

            if (string.IsNullOrEmpty(identifier))
            {
                identifier = GetFallbackId();
            }

            return identifier;
        }

        private static string GetFallbackId()
        {
            // Retrieve multiple hardware identifiers
            string cpuId = GetCpuId();
            string biosSerial = GetBiosSerialNumber();
            string baseBoardSerial = GetBaseBoardSerial();

            // Concatenate available values
            string compositeId = $"{cpuId}-{biosSerial}-{baseBoardSerial}";

            // Check if we have any valid data
            if (!string.IsNullOrEmpty(compositeId) && compositeId.Contains('-'))
            {
                return HashString(compositeId);
            }

            return "default-string";
        }

        public static string GetBiosSerialNumber()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
                foreach (var obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving BIOS Serial Number: {ex.Message}");
            }

            return null;
        }

        public static string GetBaseBoardSerial()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                foreach (var obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving BaseBoard Serial Number: {ex.Message}");
            }

            return null;
        }

        public static string GetCpuId()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                {
                    return obj["ProcessorId"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error retrieving CPU ID: {ex.Message}");
            }

            return null;
        }

        private static string HashString(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new();

            foreach (byte b in bytes)
            {
                _ = builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
