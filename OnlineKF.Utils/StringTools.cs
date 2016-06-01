using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace OnlineKF.Utils
{
    /// <summary>
    /// StringTools 的摘要说明

    /// </summary>
    public class StringTools
    {
        public StringTools()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static String ByteToHex(byte[] src)
        {
            String result = "";
            for (int i = 0; i < src.Length; i++)
            {
                result += src[i].ToString("x").PadLeft(2, '0');
            }
            return result;
        }

        /// <summary>
        /// 16进制字符串转字节数组 
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexToByte(String hexStr)
        {
            hexStr = Trim(hexStr);
            //string[] temp = System.Text.RegularExpressions.Regex.Split(str, @"[ ]+");
            byte[] hex = new byte[hexStr.Length / 2];
            int n = 0;
            for (int i = 0; i < hexStr.Length; i = i + 2)
            {
                hex[i / 2] = byte.Parse(("" + hexStr[i] + hexStr[i + 1]), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return hex;
        }

        /// <summary>
        /// 去除指定字符串两头的空格，如果字符串为空则返回空串
        /// </summary>
        /// <param name="src"></param>
        public static string Trim(string src)
        {
            if (src == null)
                return string.Empty;
            return src.Trim();
        }

        /// <summary>
        /// 判断字符串是否为空串或null
        /// </summary>
        /// <param name="src"></param>
        /// <returns>如果字符串为空串或者null则返回true</returns>
        public static bool IsBlank(String src)
        {
            if (Trim(src).Length == 0)
                return true;
            return false;
        }

        /// <summary>
        /// 字符串转换为整形，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int GetInt(string src)
        {
            try
            {
                return Convert.ToInt32(Trim(src));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转换为长整形，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static long GetLong(string src)
        {
            try
            {
                return Convert.ToInt64(Trim(src));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetShort(Object src, int len)
        {
            try
            {
                string source = src.ToString();
                source = Trim(source);

                if (CheckLength(source, len))
                    return source;
                char[] chars = source.ToCharArray();
                string result = "";
                int i = 0;
                while (CheckLength(result + chars[i], len))
                {
                    result += chars[i];
                    i++;
                }
                return result + "...";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 将空对象转换为指定字符串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string Nvl(Object src, string def)
        {
            if (src == null)
                return def;
            if (IsBlank(src.ToString()))
                return def;
            return src.ToString();
        }

        /// <summary>
        /// 计算字符串占位是否超出指定长度，中文算2位
        /// </summary>
        /// <param name="src"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool CheckLength(string src, int len)
        {
            if (StringTools.IsBlank(src))
            {
                return true;
            }

            int count = 0;
            char[] chars = src.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                //0x0020-0x007Ef（字母数字，标点符号算一位，其它 算2位）
                if (chars[i] >= 0x0020 && chars[i] <= 0x007E)
                    count++;
                else
                    count += 2;
                if (count > len)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 解析标签
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string[] GetTokens(string src)
        {
            if (StringTools.IsBlank(src))
                return new string[0];
            string[] results = src.Split(new String[] { ",", "，", ";", "；", "、" }, StringSplitOptions.RemoveEmptyEntries);
            return results;
        }

        /// <summary>
        /// 将字符串转为DateTime型
        /// </summary>
        /// <param name="src">要转化的字符串</param>
        /// <returns></returns>
        public static DateTime GetDate(String src)
        {
            if (StringTools.IsBlank(src))
            {
                return DateTime.MinValue;
            }
            else
            {
                try
                {
                    return DateTime.ParseExact(src, "yyyy-MM-dd", null);
                }
                catch (Exception ex)
                {
                    return DateTime.MinValue;
                }
            }

        }


        /// <summary>
        /// 获取中文字首字拼写
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static string GetAcronym(string chinese)
        {
            int length = chinese.Length;
            StringBuilder acronym = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                acronym.Append(GetSingle(chinese[i].ToString()));
            }
            return acronym.ToString(); ;
        }

        /// <summary>
        /// 获取单个字首字母 (GB2312)
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        private static string GetSingle(string cnChar)
        {
            byte[] arrCN = Encoding.GetEncoding("GB2312").GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.GetEncoding("GB2312").GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return string.Empty;
            }
            else return cnChar;
        }



        /// <summary>
        /// 判断字符串中是否只含有数字或字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns>false：否,true:是</returns>
        public static bool CheckLetterOrDigit(string src)
        {

            string tmpSRC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            for (int i = 0; i < src.Length; i++)
            {
                string a = src.Substring(i, 1);
                if (tmpSRC.IndexOf(a) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断字符串中是否有中文全角字符(设定密码)
        /// </summary>
        /// <param name="src"></param>
        /// <returns>false：有,true:无</returns>
        public static bool CheckChineseCode(string src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                byte[] b = System.Text.Encoding.Default.GetBytes(src.Substring(i, 1));
                if (b.Length > 1)
                    return false;

                if (src.Substring(i, 1).Equals(" "))
                    return false;
            }

            return true;
        }



        /// <summary>
        /// 获取字符串长度，1汉字=2字符
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int GetStrLength(string src)
        {
            int len = System.Text.Encoding.Default.GetByteCount(src);
            return len;
        }

        /// <summary>
        /// 如果字符为空，则返回N/A
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStrIsNA(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "N/A";
            }
            else
            {
                return str;
            }
        }


    }
}
