using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jig.Layout
{
    public static class LayoutRenderer
    {
        private const string ShortDate = "${shortdate}";
        private const string LongDate = "${longdate}";
        private const string MachineName = "${machinename}";
        private const string Userame = "${username}";

        private const string CustomDatePattern = "[$][{]customedate:{.+?)[}]";

        public static string Render(this string plainText)
        {
            var currentTime = DateTime.Now;
            var resultText = plainText;

            resultText = ReplaceShortDate(resultText, currentTime);
            resultText = ReplaceLongDate(resultText, currentTime);
            resultText = ReplaceMachineName(resultText);
            resultText = ReplaceUserName(resultText);
            resultText = ReplaceCustomDate(resultText, currentTime);

            return resultText;
        }

        private static string ReplaceShortDate(string text, DateTime currentTime)
        {
            if (text.Contains(ShortDate))
            {
                var replaceTime = currentTime.ToString("yyyyMMdd");
                return text.Replace(ShortDate, replaceTime);
            }
            return text;
        }

        private static string ReplaceLongDate(string text, DateTime currentTime)
        {
            if (text.Contains(LongDate))
            {
                var replaceTime = currentTime.ToString("yyyyMMdd_HHmmssffff");
                return text.Replace(LongDate, replaceTime);
            }
            return text;
        }

        private static string ReplaceMachineName(string text)
        {
            if (text.Contains(MachineName))
            {
                return text.Replace(MachineName, Environment.MachineName);
            }
            return text;
        }
        
        private static string ReplaceUserName(string text)
        {
            if (text.Contains(Userame))
            {
                return text.Replace(Userame, Environment.UserName);
            }
            return text;
        }

        private static string ReplaceCustomDate(string text, DateTime currentTime)
        {
            var dateFormats = Regex.Matches(text, CustomDatePattern);

            if (dateFormats.Count == 0) return text;

            var result = text;

            foreach(Match format in dateFormats)
            {
                var replaceTime = currentTime.ToString(format.Groups[0].Value);
                result = result.Replace(format.Groups[0].Value, replaceTime);
            }

            return result;
        }
    }
}
