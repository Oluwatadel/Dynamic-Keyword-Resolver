using System.Globalization;
using System.Text.RegularExpressions;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Resolve_ShouldReplaceYESTERDAYWithCorrectDate()
        {
            // Arrange
            string input = "File_YESTERDAY.log";
            string pattern = @"NOW|YESTERDAY";
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Act
            string result = Resolve(input, pattern, culture);

            // Assert
            string expectedDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", culture);
            Assert.Equal($"File_{expectedDate}.log", result);
        }

        [Fact]
        public void Resolve_ShouldReplaceNOWWithCurrentDateTime()
        {
            // Arrange
            string input = "Report_NOW.log";
            string pattern = @"NOW|YESTERDAY";
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Act
            string result = Resolve(input, pattern, culture);

            // Assert
            string expectedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture);
            Assert.StartsWith("Report_", result);
            Assert.EndsWith(".log", result);
            Assert.Contains(expectedDateTime.Substring(0, 10), result); // Check only the date part for simplicity
        }

        [Fact]
        public void Resolve_ShouldThrowExceptionForUnknownKeyword()
        {
            // Arrange
            string input = "File_UNKNOWN.log";
            string pattern = @"NOW|YESTERDAY|UNKNOWN"; // Include UNKNOWN in the pattern
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Resolve(input, pattern, culture));
        }

        // Helper methods from the original code
        private static string Resolve(string input, string pattern, CultureInfo culture)
        {
            return Regex.Replace(input, pattern, match => KeywordResolver(match.Value, culture));
        }

        private static string KeywordResolver(string keyword, CultureInfo culture)
        {
            if (keyword == "YESTERDAY") return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", culture);
            if (keyword == "NOW") return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture);
            throw new ArgumentException($"Unknown keyword: {keyword}");
        }
    }
}