using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tool
{
    public static class RegexTool
    {
        /// <summary>
        /// 判断字符串是否纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        /// <summary>
        /// 判断字符串是否纯字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetter(string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 判断字符串是否字母或数字的组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetterOrNumber(string str)
        {
            return Regex.IsMatch(str, @"(?i)^[0-9a-z]+$");
        }

        /// <summary>
        /// 统计字符串中汉字个数   
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountChinese(string str)
        {
            return str.Count(c => Regex.IsMatch(c.ToString(), @"^[\u4E00-\u9FA5]{0,}$"));
        }

        /// <summary>
        /// 判断字符串是否纯中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(string str)
        {
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5],{0,}$");
        }

        /// <summary>
        /// 判断字符串中是否包含中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 统计字符串中全角字符个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountSbcCase(string str)
        {
            return Encoding.Default.GetByteCount(str) - str.Length;
        }

        /// <summary>
        /// 判断字符串中是否包含全角字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasSbcCase(string str)
        {
            return CountSbcCase(str) > 0;
        }

        /// <summary>
        /// 统计字符串中半角字符个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountDbcCase(string str)
        {
            return str.Length - CountSbcCase(str);
        }

        /// <summary>
        /// 判断字符串中是否包含半角字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasDbcCase(string str)
        {
            return CountDbcCase(str) > 0;
        }

        /// <summary>
        /// 判断字符串中是否符合邮箱格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            return Regex.IsMatch(str, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }
}
