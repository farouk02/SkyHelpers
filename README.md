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

// To change language, set the current thread's culture:
// CultureInfo.CurrentUICulture = new CultureInfo("fr");
// relative = DateHelper.RelativeAgo(date); // "il y a 5 min"

// Hijri Date
string hijri = DateHelper.GetHijriDate(DateTime.Now); // "10 Ramadan 1445"
```

### LocalizationHelper

Internal helper for retrieving localized strings.

#### Features
- **GetString**: Retrieves a localized string key.
- **GetRelativeString**: Helper for past/future relative date strings.
- **Automatic Culture**: Always uses `CultureInfo.CurrentUICulture`.

#### Usage (Internal)
```csharp
string text = LocalizationHelper.GetString("KeyName");
```