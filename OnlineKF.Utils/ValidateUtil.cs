using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    /// <summary>
    /// 各种输入格式验证辅助类
    /// </summary>
    public class ValidateUtil
    {
        private static Regex RegNumber = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region 用户名密码格式

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string stringValue)
        {
            return Encoding.Default.GetBytes(stringValue).Length;
        }

        /// <summary>
        /// 检测用户名格式是否有效
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            int userNameLength = GetStringLength(userName);
            if (userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$"))
            {   // 判断用户名的长度（4-20个字符）及内容（只能是汉字、字母、下划线、数字）是否合法
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 密码有效性
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        #endregion

        #region 数字字符串检查

        /// <summary>
        /// 判断对象是否为Int型
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:是，fasel:否</returns>
        public static bool IsInt(Object src)
        {

            try
            {
                Convert.ToInt32(src);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 判断对象是否为Long型
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:是，fasel:否</returns>
        public static bool IsLong(Object src)
        {

            try
            {
                Convert.ToInt64(src);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 判断对象是否为Double型
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:是，fasel:否</returns>
        public static bool IsDouble(Object src)
        {

            try
            {
                Convert.ToDouble(src);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 判断对象是否为Decimal型
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:是，fasel:否</returns>
        public static bool IsDecimal(Object src)
        {

            try
            {
                Convert.ToDecimal(src);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        /// <summary>
        /// 判断字符串是否为空串或null
        /// </summary>
        /// <param name="src"></param>
        /// <returns>如果字符串为空串或者null则返回true</returns>
        public static bool IsBlank(String src)
        {
            if (src == null)
                return true;
            if (src.Trim().Length == 0)
                return true;
            return false;
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


        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        public static int GetCHZNLength(string inputData)
        {
            System.Text.ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] bytes = n.GetBytes(inputData);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;

        }

        #endregion

        #region 常用格式

        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>
        public static bool IsIdCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
            {
                return false;
            }

            if (idCard.Length == 15)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            else if (idCard.Length == 18)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static bool IsValidZip(string zip)
        {
            Regex rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhone(string phone)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(phone);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            Regex rx = new Regex(@"^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(mobile);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="strVla"></param>
        /// <returns></returns>
        public static bool IsValidPhoneAndMobile(string number)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(number);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return r.IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 验证字符串是否是GUID
        /// </summary>
        /// <param name="guid">字符串</param>
        /// <returns></returns>
        public static bool IsGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }








        #endregion

        #region 日期检查

        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDate(string strValue)
        {
            try
            {
                DateTime.Parse(strValue);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        #region 检查SQL字符串是否合法

        /// <summary>
        /// 检查SQL字符串是否合法
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <returns></returns>			
        public static bool CheckMathLength(string inputData)
        {
            bool result = false;
            if (inputData != null && inputData != string.Empty)
            {
                inputData = inputData.Trim();
                result = !Regex.IsMatch(inputData, @"[-|;|,|/|(|)|[|]|}|{|%|@|*|!|']");
            }
            return result;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string CheckMathLength(string inputData, int maxLength)
        {
            if (inputData != null && inputData != string.Empty)
            {
                inputData = inputData.Trim();
                if (inputData.Length > maxLength)//按最大长度截取字符串
                {
                    inputData = inputData.Substring(0, maxLength);
                }
            }
            return inputData;
        }        

        #endregion

        #region 过滤危险HTML方法

        /// 此处过滤危险HTML方法
        /// </summary>
        /// <param name="html">要过滤的html字符串</param>
        /// <returns></returns>
        public static string FilterHTML(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";

            #region 过滤 script
            System.Text.RegularExpressions.Regex regex_script1 = new System.Text.RegularExpressions.Regex("(<script[\\s\\S]*?\\/script\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_script2 = new System.Text.RegularExpressions.Regex("(<(script[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_script1.Replace(html, "");
            html = regex_script1.Replace(html, "");
            #endregion 过滤 script


            #region 过滤 <iframe> 标签
            System.Text.RegularExpressions.Regex regex_iframe1 = new System.Text.RegularExpressions.Regex("(<iframe [\\s\\S]*?\\/iframe\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_iframe2 = new System.Text.RegularExpressions.Regex("(<(iframe [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_iframe1.Replace(html, "");
            html = regex_iframe2.Replace(html, "");
            #endregion 过滤 <iframe> 标签

            #region 过滤 <frameset> 标签
            System.Text.RegularExpressions.Regex regex_frameset1 = new System.Text.RegularExpressions.Regex("(<frameset [\\s\\S]*?\\/frameset\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frameset2 = new System.Text.RegularExpressions.Regex("(<(frameset [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frameset1.Replace(html, "");
            html = regex_frameset2.Replace(html, "");
            #endregion 过滤 <frameset> 标签

            #region 过滤 <frame> 标签
            System.Text.RegularExpressions.Regex regex_frame1 = new System.Text.RegularExpressions.Regex("(<frame[\\s\\S]*?\\/frame\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frame2 = new System.Text.RegularExpressions.Regex("(<(frame[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frame1.Replace(html, "");
            html = regex_frame2.Replace(html, "");
            #endregion 过滤 <frameset> 标签

            #region 过滤 <form> 标签
            System.Text.RegularExpressions.Regex regex_form1 = new System.Text.RegularExpressions.Regex("(<(form [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_form2 = new System.Text.RegularExpressions.Regex("(<(/form[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_form1.Replace(html, "");
            html = regex_form2.Replace(html, "");
            #endregion 过滤 <form> 标签

            #region 过滤 on: 的事件

            //过滤on 带单引号的 过滤on  带双引号的 过滤on 不带有引号的
            html = GetReplace(html, "<[\\s\\S]+ (on)[a-zA-Z]{4,20} *= *[\\S ]{3,}>",
                "((on)[a-zA-Z]{4,20} *= *'[^']{3,}')|((on)[a-zA-Z]{4,20} *= *\"[^\"]{3,}\")|((on)[a-zA-Z]{4,20} *= *[^>/ ]{3,})", "");

            #endregion 过滤 on: 的事件

            #region 过滤 javascript: 的事件

            html = GetReplace(html, "<[\\s\\S]+ (href|src|background|url|dynsrc|expression|codebase) *= *[ \"\']? *(javascript:)[\\S]{1,}>"
                , "(' *(javascript|vbscript):([\\S^'])*')|(\" *(javascript|vbscript):[\\S^\"]*\")|([^=]*(javascript|vbscript):[^/> ]*)", "#");
            #endregion 过滤 javascript: 的事件

            return html;
        }


        /// <summary>
        /// 正则双重过滤
        /// </summary>
        /// <param name="content"></param>
        /// <param name="splitKey1"></param>
        /// <param name="splitKey2"></param>
        /// <param name="newChars"></param>
        /// <returns></returns>
        public static string GetReplace(string content, string splitKey1, string splitKey2, string newChars)
        {
            //splitKey1 第一个正则式匹配  

            //splitKey2 匹配结果中再次匹配进行替换  

            if (splitKey1 != null && splitKey1 != "" && splitKey2 != null && splitKey2 != "")
            {
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(splitKey1);
                System.Text.RegularExpressions.MatchCollection mc = rg.Matches(content);

                foreach (System.Text.RegularExpressions.Match mc1 in mc)
                {
                    string oldChar = mc1.ToString();
                    string newChar = new System.Text.RegularExpressions.Regex(splitKey2, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace(oldChar, newChars);
                    content = content.Replace(oldChar, newChar);
                }
                return content;
            }
            else
            {
                if (splitKey2 != null && splitKey2 != "")
                {
                    System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(splitKey2, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    return rg.Replace(content, newChars);
                }
            }
            return content;
        }

        #endregion
    }
}
