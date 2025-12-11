# SkyHelpers

A collection of useful C# helpers and extensions.

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

#### Usage
```csharp
using SkyHelpers;

int? id = Parse.Int("123");
decimal price = Parse.Decimal("19.99");
double lat = Parse.Double("48.8566");
bool isActive = Parse.Boolean("true");
DateTime? date = Parse.DateTime("2023-01-01");
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

### WhatsApp

A simple wrapper around Twilio for sending WhatsApp messages.

#### Properties
- **SendMessageAsync**: Sends a WhatsApp message from the configured sender to a recipient.

#### Usage
```csharp
using SkyHelpers;

var wa = new WhatsApp("AC...", "AuthToken...", "+14155238886");
await wa.SendMessageAsync("+15551234567", "Hello World!");
```

```

### Sms

A simple wrapper around Twilio for sending SMS messages.

#### Methods
- **SendAsync**: Sends an SMS message from the configured sender to a recipient. Handles number formatting (defaults to +213 for Algeria).

#### Usage
```csharp
using SkyHelpers;

var sms = new Sms("AC...", "AuthToken...", "+15550101234");
await sms.SendAsync("+15559876543", "Hello World!");
await sms.SendAsync("0550123456", "Hello Algeria!"); // Auto-formats to +213
```

### ImageHelper

Utilities for manipulating images, converting bytes, and saving files.

#### Features
- **GetImageBytesFromPath**: Safely reads image bytes from a path.
- **GetImageFromPath**: Loads an `Image` object from a path.
- **SaveImageToProjectDirectory**: Saves an image to `[AppDir]/Images/[SubDirectory]`.
- **ImageToBytes / BytesToImage**: Convenient conversion methods.
- **To24bppRgb**: Standardization.

#### Usage
```csharp
using SkyHelpers;
using System.Drawing;

Image img = ImageHelper.GetImageFromPath("path/to/image.jpg");
byte[] bytes = ImageHelper.ImageToBytes(img);
```
