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

// Relative Time (English by default or current culture)
var date = DateTime.Now.AddMinutes(-5);
string relative = DateHelper.RelativeAgo(date); // "5 min ago" (en)

// Explicit Culture (French)
var frCulture = new CultureInfo("fr");
string relativeFr = DateHelper.RelativeAgo(date, frCulture); // "il y a 5 min"

// Explicit Culture (Arabic)
var arCulture = new CultureInfo("ar");
string relativeAr = DateHelper.RelativeAgo(date, arCulture); // "منذ 5 دقيقة"

// Hijri Date
string hijri = DateHelper.GetHijriDate(DateTime.Now); // "10 Ramadan 1445"
string hijriAr = DateHelper.GetHijriDate(DateTime.Now, culture: arCulture); // "10 رمضان 1445"
```