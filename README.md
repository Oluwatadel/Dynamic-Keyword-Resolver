# Dynamic Keyword Resolver

A lightweight C# library for resolving dynamic date-related keywords in strings, designed for use cases like FTP file processing, where file names contain placeholders (e.g., `NOW`, `YESTERDAY`). The library replaces these placeholders with actual date and time values based on user-defined logic.

---

## Features
- **Keyword Parsing**: Supports dynamic date-related keywords like:
  - `NOW`, `YESTERDAY`, `TODAY`
  - Relative offsets (e.g., `NOW-1d`, `NOW+2h`)
  - Formatted dates (e.g., `Format(NOW, "yyyy-MM-dd")`)
- **Localization Support**: Resolves keywords using culture-specific date and time formats.
- **Lightweight**: Minimal dependencies, easy to integrate into any project.
- **Flexible**: Extensible to handle additional custom keywords.

---

## Installation
Add the library to your project using NuGet:
```bash
dotnet add package DynamicKeywordResolver
using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        string input = "File_YESTERDAY_Report_NOW.log";
        string pattern = @"NOW(?:[+-]\d+[smhd])?|YESTERDAY|TODAY|Format\(NOW, ""[^""]+""\)";
        CultureInfo culture = CultureInfo.InvariantCulture;

        string result = KeywordResolver.Resolve(input, pattern, culture);
        Console.WriteLine(result);
        // Output: "File_2024-12-25_Report_2024-12-26.log"
    }
}

## Supported Keywords

| Keyword         | Description                                  | Example Output               |
|------------------|----------------------------------------------|------------------------------|
| `NOW`           | Current date and time.                      | `2024-12-26 14:30:00`        |
| `YESTERDAY`     | Current date minus one day.                 | `2024-12-25`                 |
| `NOW-1d`        | Current date minus 1 day.                   | `2024-12-25 14:30:00`        |
| `NOW+2h`        | Current time plus 2 hours.                  | `2024-12-26 16:30:00`        |
| `Format(NOW)`   | Custom date forma


public static string KeywordResolver(string keyword, CultureInfo culture)
{
    if (keyword == "YESTERDAY") return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", culture);
    if (keyword == "NOW") return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture);
    // Add custom keyword handling here
    throw new ArgumentException($"Unknown keyword: {keyword}");
}

## Extending the Library

You can add support for additional keywords by modifying the `KeywordResolver` method:

```csharp
public static string KeywordResolver(string keyword, CultureInfo culture)
{
    if (keyword == "YESTERDAY") 
        return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", culture);
    if (keyword == "NOW") 
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture);
    // Add custom keyword handling here
    throw new ArgumentException($"Unknown keyword: {keyword}");
}

##Unit Tests
The library includes unit tests to ensure reliability. Run the tests using the .NET CLI or Visual Studio Test Explorer.

Example Test Cases
```csharp
Copy code
[Fact]
public void Resolve_ShouldReplaceYESTERDAYWithCorrectDate()
{
    string input = "File_YESTERDAY.log";
    string pattern = @"NOW|YESTERDAY";
    string expected = $"File_{DateTime.Now.AddDays(-1):yyyy-MM-dd}.log";
    
    Assert.Equal(expected, KeywordResolver.Resolve(input, pattern, CultureInfo.InvariantCulture));
}

##Contributing
Contributions are welcome! If you’d like to add features or fix bugs, follow these steps:

Fork the repository.
Create a feature branch: git checkout -b feature-name.
Commit your changes: git commit -m "Description of changes".
Push to the branch: git push origin feature-name.
Open a pull request.

##Contact
For questions or feedback, feel free to reach out via GitHub or email.
