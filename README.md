# SkyHelpers

A collection of useful C# helpers and extensions for **Windows Forms** applications.

## Platform Requirements

- **Target Framework**: .NET 10.0 (Windows)
- **Platform**: Windows only (uses Windows Forms)

## Helpers

### DateHelper

A utility class for friendly date formatting and Hijri date conversion.

#### Features
- **Relative Time**: Converts `DateTime` to human-readable strings (e.g., "just now", "5 minutes ago", "in 2 days").
- **Hijri Date**: Converts Gregorian dates to Hijri dates with support for adjustment and localized month names.
- **Localization**: Supports English, French, and Arabic.

#### Usage

```csharp
using SkyHelpers;
using System.Globalization;

// Relative Time (Uses current UI culture)
var date = DateTime.Now.AddMinutes(-5);
string relative = DateHelper.RelativeAgo(date); // "5 min ago" (if en)

// Explicit Culture (French)
var frCulture = new CultureInfo("fr");
string relativeFr = DateHelper.RelativeAgo(date, frCulture); // "il y a 5 min"

// Hijri Date
string hijri = DateHelper.GetHijriDate(DateTime.Now); // "10 Ramadan 1445"
string hijriAr = DateHelper.GetHijriDate(DateTime.Now, culture: new CultureInfo("ar")); // "10 رمضان 1445"
```

### LocalizationHelper

Internal helper for retrieving localized strings.

#### Features
- **GetString**: Retrieves a localized string key.
- **GetRelativeString**: Helper for past/future relative date strings.
- **Culture Support**: Accepts optional `CultureInfo`, defaults to `CurrentUICulture`.

#### Usage (Internal)
```csharp
string text = LocalizationHelper.GetString("KeyName");
string textFr = LocalizationHelper.GetString("KeyName", new CultureInfo("fr"));
```

### Parse

Safe parsing utilities for common types.

#### Features
- **Int**: Parses to `int?`. Returns `null` on failure.
- **Decimal**: Parses to `decimal`. Returns `0` on failure.
- **Double**: Parses to `double`. Returns `0` on failure.
- **Boolean**: Parses to `bool`. Returns `false` on failure.
- **DateTime**: Parses to `DateTime?`. Returns `null` on failure.
- **TimeOnly**: Parses to `TimeOnly?`. Returns `null` on failure.
- **DateOnly**: Parses to `DateOnly?`. Returns `null` on failure.

#### Usage
```csharp
using SkyHelpers;

int? id = Parse.Int("123");
decimal price = Parse.Decimal("19.99");
double lat = Parse.Double("48.8566");
bool isActive = Parse.Boolean("true");
DateTime? date = Parse.DateTime("2023-01-01");
TimeOnly? time = Parse.TimeOnly("14:30");
DateOnly? dateOnly = Parse.DateOnly("2023-12-25");
```

### Hooks

Interfaces for common entity framework entity lifecycle hooks.

#### Features
- **IBeforeCreate**: `void BeforeCreate()`
- **IBeforeUpdate**: `void BeforeUpdate()`
- **IBeforeDelete**: `void BeforeDelete()`

#### Usage
```csharp
using SkyHelpers;

public class MyEntity : IBeforeCreate
{
    public void BeforeCreate()
    {
        // Logic before creation
    }
}
```

### PasswordHelper

BCrypt password hashing compatible with Laravel (`$2y$` prefix).

#### Features
- **HashPassword**: Hashes a password with BCrypt (default cost 10) and uses `$2y$` prefix.
- **VerifyPassword**: Verifies a password against a hash (supports `$2y$`, `$2a$`, `$2b$`).

#### Usage
```csharp
using SkyHelpers;

string hash = PasswordHelper.HashPassword("secret");
bool isValid = PasswordHelper.VerifyPassword("secret", hash);
```

### Messages

A unified Twilio wrapper for sending WhatsApp and SMS messages.

#### Configuration
The `Messages` class is initialized with your Twilio credentials:
- **accountSid**: Your Twilio Account SID.
- **authToken**: Your Twilio Auth Token.
- **fromNumber**: The Twilio phone number to send from.
- **phoneCode** (Optional): Default country dialing code (e.g., "+213"). Defaults to `+213`.

#### Methods
- **SendWhatsApp**: Sends a WhatsApp message. Returns `true` on success, `false` on failure.
- **SendSms**: Sends an SMS message. Returns `true` on success, `false` on failure.

Both methods automatically handle phone number formats (`+`, `00`, and local numbers with leading `0`).

#### Usage
```csharp
using SkyHelpers;

var msg = new Messages("AC...", "AuthToken...", "+14155238886");

// Send WhatsApp
bool whatsAppSent = msg.SendWhatsApp("+15551234567", "Hello via WhatsApp!");
if (whatsAppSent)
    Console.WriteLine("WhatsApp sent!");

// Send SMS (requires Messaging Service SID)
bool smsSent = msg.SendSms("0550123456", "Hello via SMS!", "MG...");
if (smsSent)
    Console.WriteLine("SMS sent!");
```

### PhoneCode

A utility class providing a comprehensive list of international dialing codes.

#### Features
- **GetAll / GetCountryPhoneCodes**: Returns a list of `CountryInfo` objects containing country names and dialing codes (e.g., "Algeria", "+213").

#### Usage
```csharp
using SkyHelpers;

var codes = PhoneCode.GetAll();
var dz = codes.FirstOrDefault(c => c.Name == "Algeria");
Console.WriteLine(dz.Code); // Output: +213
```

### MachineIdentifier

Generates a unique hardware-based identifier for the machine running the application.

#### Features
- **GetUniqueIdentifier**: Returns a hashed string derived from CPU ID, BIOS Serial Number, and BaseBoard Serial Number.
- **Fallbacks**: Robustly handles missing hardware information by trying multiple identifiers.

#### Usage
```csharp
using SkyHelpers;

string uniqueId = MachineIdentifier.GetUniqueIdentifier();
Console.WriteLine(uniqueId); // e.g., "a1b2c3d4..."
```

### ImageHelper

Utilities for manipulating images, converting bytes, and saving files.

#### Features
- **GetImageBytesFromPath**: Safely reads image bytes from a path.
- **GetImageFromPath**: Loads an `Image` object from a path.
- **SaveImageToProjectDirectory**: Saves an image to `[AppDir]/Images/[SubDirectory]`.
- **ImageToBytes / BytesToImage**: Convenient conversion methods.
- **To24bppRgb**: Standardization.
- **ResizeImage**: Resizes an image while preserving aspect ratio and optimizing output format (JPEG vs PNG).

#### Usage
```csharp
using SkyHelpers;
using System.Drawing;

Image img = ImageHelper.GetImageFromPath("path/to/image.jpg");
byte[] bytes = ImageHelper.ImageToBytes(img);

// Resizing
byte[] originalBytes = File.ReadAllBytes("large_image.jpg");
// Resize to 800px width (height calculated automatically)
byte[] resizedBytes = ImageHelper.ResizeImage(originalBytes, width: 800); 
```

### ImageCropperForm

A Windows Forms dialog for interactive image cropping with zoom and pan support.

#### Features
- **Square Crop**: Fixed 400x400px square crop area with dark overlay.
- **Zoom Control**: Slider to zoom in/out (50% - 300%).
- **Pan Support**: Drag the image to position it within the crop area.
- **High-Quality Output**: Uses bicubic interpolation for smooth cropping.

#### Properties
- **CroppedImage**: Returns the cropped `Image` after confirmation.

#### Usage
```csharp
using SkyHelpers.Common;
using System.Drawing;

Image originalImage = Image.FromFile("photo.jpg");
using var cropper = new ImageCropperForm(originalImage);

if (cropper.ShowDialog() == DialogResult.OK)
{
    Image croppedImage = cropper.CroppedImage;
    croppedImage.Save("cropped_photo.jpg");
}
```
