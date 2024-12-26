using System.Globalization;
using System.Text.RegularExpressions;

namespace NamaResolver.Logic
{
    public static class NameResolver
    {
        public static string Resolve(string input, CultureInfo culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;

            string pattern = @"NOW(?:[+-]\d+[smhd])?|YESTERDAY|TODAY|Format\([^,]+, ""[^""]+""\)";
            return Regex.Replace(input, pattern, match 
                => KeywordResolver(match.Value, culture)
                );            
        }


        private static string KeywordResolver(string keyword, CultureInfo cultureInfo)
        {
            var dateTime = DateTime.Now;

            if(keyword.StartsWith("YESTERDAY") || keyword.EndsWith("YESTERDAY"))
            {
                return dateTime.AddDays(-1).ToString("yyyy-mm-dd", cultureInfo);
            }
            else if (keyword.StartsWith("TODAY") || keyword.EndsWith("TODAY"))
            {
                return dateTime.ToString("yyyy-mm-dd", cultureInfo);
            }

            else if (keyword.StartsWith("NOW") || keyword.EndsWith("NOW"))
            {
                return ResolveNoWSpecified(keyword, dateTime, cultureInfo);
            }
            else if (keyword.StartsWith("FORMAT(") || keyword.EndsWith("FORMAT("))
            {
                return ResolveFormatNoWSpecified(keyword, dateTime, cultureInfo);
            }
            throw new ArgumentException($"Invalid format syntax: {keyword}");
        }

        private static string ResolveNoWSpecified(string keyword, DateTime dateTime, CultureInfo cultureInfo)
        {
            Match match = Regex.Match(keyword, @"NOW([+-]\d+)([smhd])");
            if (!match.Success) return dateTime.ToString("yyyy-MM-dd", cultureInfo);

            int offset = int.Parse(match.Groups[1].Value.Trim();
            char unit = match.Groups[2].Value[0];

            if (unit == 's') return dateTime.AddSeconds(offset).ToString("yyyy-MM-dd HH:mm:ss", cultureInfo);
            else if (unit == 'm') return dateTime.AddMinutes(offset).ToString("yyyy-MM-dd HH:mm:ss", cultureInfo);
            else if (unit == 'h') return dateTime.AddHours(offset).ToString("yyyy-MM-dd HH:mm:ss", cultureInfo);
            else if (unit == 'd') return dateTime.AddDays(offset).ToString("yyyy-MM-dd HH:mm:ss", cultureInfo);
            else throw new ArgumentException($"{unit} is not a supported time unit");
        }

        private static string ResolveFormatNoWSpecified(string keyword, DateTime dateTime, CultureInfo cultureInfo)
        {
            Match match = Regex.Match(keyword, @"Format\(([^,]+), ""([^""]+)""\)"));

            if (!match.Success)
            {
                throw new ArgumentException($"Invalid format syntax: {keyword}");
            };
            var innerExpression = match.Groups[1].Value.Trim();
            var format = match.Groups[2].Value;

            DateTime resolvedDate;
            
            if (innerExpression == "NOw") resolvedDate = dateTime;
            else if (innerExpression == "YESTERDAY") resolvedDate = dateTime.AddDays(-1);
            else if (innerExpression == "TODAY") resolvedDate = dateTime;
            else if (innerExpression.StartsWith("TODAY") || innerExpression.EndsWith("TODAY")) 
                resolvedDate = DateTime.Parse(ResolveNoWSpecified(innerExpression,dateTime,cultureInfo), cultureInfo);
            else throw new ArgumentException($"Unsupported expression in Format: {innerExpression}");

            return resolvedDate.ToString(format, cultureInfo);
        }
    }
}
