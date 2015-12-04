using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Zkly.Common.Extension
{
    public static class StringExt
    {
        /// <summary>
        /// Check if the string is null, if yes, return an empty string instead.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a string</returns>
        public static string TrimNull(this string theSource)
        {
            return theSource ?? string.Empty;
        }

        /// <summary>
        /// Check if the string is null or empty, if yes, return the replace string instead.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theReplace">the replace string</param>
        /// <returns>a string</returns>
        public static string TrimNull(this string theSource, string theReplace)
        {
            return string.IsNullOrEmpty(theSource) ? theReplace : theSource;
        }

        /// <summary>
        /// Check if the string is null or empty, if yes, return the replace string instead.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="replacer">the replace function</param>
        /// <returns>a string</returns>
        public static string TrimNull(this string theSource, Func<string> replacer)
        {
            return string.IsNullOrEmpty(theSource) ? replacer() : theSource;
        }

        /// <summary>
        /// check if the string is equals with any of the compare string.
        /// </summary>
        /// <param name="theSource">the source string</param>
        /// <param name="compares">caompare strings</param>
        /// <returns>true or false</returns>
        public static bool EqualsAny(this string theSource, params string[] compares)
        {
            return compares.Any(s => string.Equals(theSource, s));
        }

        /// <summary>
        /// check if the string is equals with any of the compare string.
        /// </summary>
        /// <param name="theSource">the source string</param>
        /// <param name="comparison">comparison type</param>
        /// <param name="compares">caompare strings</param>
        /// <returns>true or false</returns>
        public static bool EqualsAny(this string theSource, StringComparison comparison, params string[] compares)
        {
            return compares.Any(s => string.Equals(theSource, s, comparison));
        }

        /// <summary>
        /// get text from HTML
        /// </summary>
        /// <param name="source">HTML</param>
        /// <returns>text</returns>
        public static string StripHtml(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var content = Regex.Replace(source, @"<(script|style).*?</\1>", string.Empty);
            content = Regex.Replace(content, @"<.*?>", string.Empty);
            content = HttpUtility.HtmlDecode(content);
            return content;
        }

        #region HyphenWords
        /// <summary>
        /// Hyphen words in a string, typically for SEO URL transform, will transform "some $ugly ###url wit[]h spaces" into "some-ugly-url-with-spaces"
        /// </summary>
        public static string HyphenWords(this string source)
        {
            return HyphenWords(source, 10);
        }

        /// <summary>
        /// Hyphen words in a string, typically for SEO URL transform, will transform "some $ugly ###url wit[]h spaces" into "some-ugly-url-with-spaces"
        /// </summary>
        public static string HyphenWords(this string source, int maxWords)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            // remove invalid chars, make into spaces   
            string str = InvalidCharsRegex.Replace(source, string.Empty);
            str = SingleSpaceRegex.Replace(str, " ").Trim();
            str = str.Split(' ').Take(maxWords).Join("-");
            return str;
        }

        private static readonly Regex InvalidCharsRegex = new Regex(@"[^a-zA-Z0-9\s-]", RegexOptions.Compiled);
        private static readonly Regex SingleSpaceRegex = new Regex(@"[\s-]+", RegexOptions.Compiled);
        #endregion

        #region Regex Match
        /// <summary>
        /// search in a string and find the first matched element
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="pattern">regex match format pattern</param>
        /// <returns>matched string</returns>
        public static string Match(this string source, string pattern)
        {
            return source.Match(pattern, RegexOptions.None);
        }

        /// <summary>
        /// search in a string and find the first matched element
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="pattern">regex match format pattern</param>
        /// <param name="option">A bitwise OR combination of the enumeration values for Regular Expression.</param>
        /// <returns>matched string</returns>
        public static string Match(this string source, string pattern, RegexOptions option)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return source;
            }

            var match = Regex.Match(source, pattern, option);
            if (match.Success)
            {
                return match.Groups.Count > 1 ? match.Groups[1].Value : match.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// search in a string and find all matched elements
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="pattern">regex match format pattern</param>
        /// <returns>matched strings</returns>
        public static string[] Matches(this string source, string pattern)
        {
            return source.Matches(pattern, RegexOptions.None);
        }

        /// <summary>
        /// search in a string and find all matched elements
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="pattern">regex match format pattern</param>
        /// <param name="option">A bitwise OR combination of the enumeration values for Regular Expression.</param>
        /// <returns>matched strings</returns>
        public static string[] Matches(this string source, string pattern, RegexOptions option)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new string[0];
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return new[] { source };
            }

            var matches = Regex.Matches(source, pattern, option);
            return (from Match m in matches
                    where m.Success
                    from g in m.Groups.Cast<Group>().Skip(m.Groups.Count > 1 ? 1 : 0)
                    select g.Value).ToArray();
        }
        #endregion

        #region Convert
        /// <summary>
        /// convert string to a bool value if possible, if failed, return false as default value.
        /// <para>only true, yes or 1 will be treated to true value</para>
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a bool value</returns>
        public static bool ToBoolean(this string theSource)
        {
            return theSource.EqualsAny(StringComparison.OrdinalIgnoreCase, "true", "yes", "1");
        }

        /// <summary>
        /// convert string to a int value if possible, if failed, return 0 as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a Int32 value</returns>
        public static int ToInt(this string theSource)
        {
            return theSource.ToInt(0);
        }

        /// <summary>
        /// convert string to a int value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to int</param>
        /// <returns>a Int32 value</returns>
        public static int ToInt(this string theSource, int theDefault)
        {
            int tempValue;
            return int.TryParse(theSource.TrimNull(), out tempValue) ? tempValue : theDefault;
        }

        /// <summary>
        /// convert string to a long value if possible, if failed, return 0 as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a Int64 value</returns>
        public static long ToLong(this string theSource)
        {
            return theSource.ToLong(0);
        }

        /// <summary>
        /// convert string to a long value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to long</param>
        /// <returns>a Int64 value</returns>
        public static long ToLong(this string theSource, long theDefault)
        {
            long tempValue;
            return long.TryParse(theSource.TrimNull(), out tempValue) ? tempValue : theDefault;
        }

        /// <summary>
        /// convert string to a float value if possible, if failed, return 0 as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a float value</returns>
        public static float ToFloat(this string theSource)
        {
            return theSource.ToFloat(0);
        }

        /// <summary>
        /// convert string to a float value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to float</param>
        /// <returns>a float value</returns>
        public static float ToFloat(this string theSource, float theDefault)
        {
            float tempValue;
            return float.TryParse(theSource.TrimNull("0"), out tempValue) ? tempValue : theDefault;
        }

        /// <summary>
        /// convert string to a double value if possible, if failed, return 0d as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a double value</returns>
        public static double ToDouble(this string theSource)
        {
            return theSource.ToDouble(0d);
        }

        /// <summary>
        /// convert string to a double value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to double</param>
        /// <returns>a double value</returns>
        public static double ToDouble(this string theSource, double theDefault)
        {
            double tempValue;
            return double.TryParse(theSource.TrimNull("0"), out tempValue) ? tempValue : theDefault;
        }

        /// <summary>
        /// convert string to a decimal value if possible, if failed, return 0 as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a decimal value</returns>
        public static decimal ToDecimal(this string theSource)
        {
            return theSource.ToDecimal(0);
        }

        /// <summary>
        /// convert string to a decimal value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to decimal</param>
        /// <returns>a decimal value</returns>
        public static decimal ToDecimal(this string theSource, decimal theDefault)
        {
            decimal tempValue;
            return decimal.TryParse(theSource.TrimNull("0"), out tempValue) ? tempValue : theDefault;
        }

        /// <summary>
        /// convert string to a DateTime value if possible, if failed, return DateTime.MinValue as default value.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <returns>a DateTime value</returns>
        public static DateTime ToDateTime(this string theSource)
        {
            return theSource.ToDateTime(DateTime.MinValue);
        }

        /// <summary>
        /// convert string to a DateTime value if possible.
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theDefault">the replace value if string could not convert to DateTime</param>
        /// <returns>a DateTime value</returns>
        public static DateTime ToDateTime(this string theSource, DateTime theDefault)
        {
            DateTime tempValue;
            return !string.IsNullOrEmpty(theSource) && DateTime.TryParse(theSource, out tempValue) ? tempValue : theDefault;
        }

        public static T ToEnum<T>(this string theSource)where T:struct
        {
            T obj;
            Enum.TryParse(theSource, true, out obj);
            return obj;
        }
#endregion

        //add by yungchu
        public static bool IsOverlength(this string theSource, int length)
        {
            return !string.IsNullOrEmpty(theSource) && theSource.Length > length;
        }
    }
}
