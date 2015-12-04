using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zkly.Common.Utils
{
    public static class ValidateUtil
    {
        public const string CHINESEPATTERN = @"^[\u4e00-\u9fa5]+$";
        public const string PHONEPATTERN = @"^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
        public const string MOBILEPHONEPATTERN = @"^1\d{10}$";
        public const string NUMBERPATTERN = @"^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
        public const string NOTNATIVEPATTERN = @"^\d+$";
        public const string UINTPATTERN = @"^[0-9]*[1-9][0-9]*$";
        public const string ENGLISHPATTERN = @"^[A-Za-z]+$";
        public const string EMAILPATTERN = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string ENGANDNUMPATTERN = @"^[A-Za-z0-9]+$";
        public const string URLPATTERN = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";

        public static bool IsValidEmail(string strIn)
        {
            var validator = new EmailAddressAttribute();
            return validator.IsValid(strIn);
        }

        public static bool IsValidUrl(string strIn)
        {
            var validator = new UrlAttribute();
            return validator.IsValid(strIn);
        }

        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        public static bool IsChinese(string input)
        {
            return IsMatch(CHINESEPATTERN, input);
        }

        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
        /// 也可以没有间隔
        /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
        /// </summary>
        public static bool IsPhone(string input)
        {
            string pattern = PHONEPATTERN;
            return IsMatch(pattern, input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号
        /// </summary>
        public static bool IsMobilePhone(string input)
        {
            return IsMatch(MOBILEPHONEPATTERN, input);
        }

        /// <summary>
        /// 判断输入的字符串只包含数字
        /// 可以匹配整数和浮点数
        /// ^-?\d+$|^(-?\d+)(\.\d+)?$
        /// </summary>
        public static bool IsNumber(string input)
        {
            string pattern = NUMBERPATTERN;
            return IsMatch(pattern, input);
        }

        /// <summary>
        /// 匹配非负整数
        /// </summary>
        public static bool IsNotNagtive(string input)
        {
            return IsMatch(NOTNATIVEPATTERN, input);
        }

        // 匹配正整数
        public static bool IsUint(string input)
        {
            return IsMatch(UINTPATTERN, input);
        }

        //判断输入的字符串字包含英文字母
        public static bool IsEnglish(string input)
        {
            return IsMatch(ENGLISHPATTERN, input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        public static bool IsEmail(string input)
        {
            string pattern = EMAILPATTERN;
            return IsMatch(pattern, input);
        }

        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        public static bool IsEngAndNum(string input)
        {
            return IsMatch(ENGANDNUMPATTERN, input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个超链接
        /// </summary>
        public static bool IsURL(string input)
        {
            string pattern = URLPATTERN;
            return IsMatch(pattern, input);
        }

        #region 正则的通用方法
        /// <summary>
        /// 计算字符串的字符长度，一个汉字字符将被计算为两个字符
        /// </summary>
        /// <param name="input">需要计算的字符串</param>
        /// <returns>返回字符串的长度</returns>
        public static int GetCount(string input)
        {
            return Regex.Replace(input, @"[\u4e00-\u9fa5/g]", "aa").Length;
        }

        /// <summary>
        /// 调用Regex中IsMatch函数实现一般的正则表达式匹配
        /// </summary>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(string pattern, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        #endregion
    }
}
