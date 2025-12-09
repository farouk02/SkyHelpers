using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SkyHelpers;

public class ImageHelper
{
    public static byte[]? GetImageBytesFromPath(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
        {
            return null;
        }

        try
        {
            return File.ReadAllBytes(imagePath);
        }
        catch
        {
            // Silent failure or log properly if logger was available.
            return null;
        }
    }

    // Method to get Image object from a file path
    public static Image? GetImageFromPath(string imagePath)
    {
        byte[]? imageBytes = GetImageBytesFromPath(imagePath);

        if (imageBytes != null && imageBytes.Length > 0)
        {
            using MemoryStream ms = new(imageBytes);
            return Image.FromStream(ms);
        }

        return null;
    }

    // Method to save an Image to the project directory and return the file path
    public static string SaveImageToProjectDirectory(Image image, string subDirectory, string fileName)
    {
        ArgumentNullException.ThrowIfNull(image);

        if (string.IsNullOrEmpty(subDirectory))
            throw new ArgumentNullException(nameof(subDirectory));

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));

        // Get the project directory (where the executable is running)
        string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Combine the project directory with the sub-directory
        string fullDirectoryPath = Path.Combine(projectDirectory, "Images", subDirectory);

        // Ensure the directory exists
        if (!Directory.Exists(fullDirectoryPath))
        {
            _ = Directory.CreateDirectory(fullDirectoryPath);
        }

        // Construct the full file path
        string filePath = Path.Combine(fullDirectoryPath, fileName);

        // Save the image as a PNG file (or change format as needed)
        image.Save(filePath, ImageFormat.Png);

        return filePath;
    }

    public static Image? BytesToImage(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes.Length == 0)
            return null;

        using var ms = new MemoryStream(imageBytes);
        return Image.FromStream(ms);
    }

    public static Bitmap To24bppRgb(Image src)
    {
        var bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
        using var g = Graphics.FromImage(bmp);
        g.DrawImage(src, 0, 0, src.Width, src.Height);
        return bmp;
    }

    public static bool BytesEqual(byte[]? a, byte[]? b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (a is null || b is null)
            return false;
        if (a.Length != b.Length)
            return false;
        // .NET Core+: fast path
        return a.AsSpan().SequenceEqual(b);
    }

    public static byte[] ImageToBytes(Image img)
    {
        using var ms = new MemoryStream();

        // Clone to detach from any underlying stream used by the control
        using var cloned = new Bitmap(img);

        // If the image has alpha, save as PNG; otherwise JPEG
        var hasAlpha = Image.IsAlphaPixelFormat(cloned.PixelFormat);

        if (hasAlpha)
        {
            cloned.Save(ms, ImageFormat.Png);
        }
        else
        {
            using var rgb = To24bppRgb(cloned); // ensure non-indexed, 24bpp
            rgb.Save(ms, ImageFormat.Jpeg);
        }

        return ms.ToArray();
    }
}
