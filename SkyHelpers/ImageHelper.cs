using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

    public static byte[] ResizeImage(byte[] imageBytes, int? width = null, int? height = null)
    {
        if (imageBytes == null || imageBytes.Length == 0)
            return null;

        if (width == null && height == null)
            return imageBytes; // No resize needed

        try
        {
            using var ms = new MemoryStream(imageBytes);
            using var image = Image.FromStream(ms);
            
            // Check if source has transparency
            bool hasAlpha = Image.IsAlphaPixelFormat(image.PixelFormat);

            int newWidth;
            int newHeight;

            if (width.HasValue && height.HasValue)
            {
                newWidth = width.Value;
                newHeight = height.Value;
            }
            else if (width.HasValue)
            {
                newWidth = width.Value;
                var aspectRatio = (double)image.Height / image.Width;
                newHeight = (int)(newWidth * aspectRatio);
            }
            else
            {
                newHeight = height.Value;
                var aspectRatio = (double)image.Width / image.Height;
                newWidth = (int)(newHeight * aspectRatio);
            }

            // Use appropriate pixel format
            var format = hasAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
            using var resizedImage = new Bitmap(newWidth, newHeight, format);
            using var graphics = Graphics.FromImage(resizedImage);
            
            // Set high quality resizing settings
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var attributes = new ImageAttributes();
            attributes.SetWrapMode(WrapMode.TileFlipXY);

            graphics.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight),
                0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

            using var outStream = new MemoryStream();
            if (hasAlpha)
            {
                resizedImage.Save(outStream, ImageFormat.Png);
            }
            else
            {
                resizedImage.Save(outStream, ImageFormat.Jpeg);
            }
            return outStream.ToArray();
        }
        catch
        {
            // Return original image if resize fails, or null depending on requirement. 
            // For now, logging isn't available, so we return null to avoid bad data.
            return null;
        }
    }
}
