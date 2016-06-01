using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    /// <summary>
    /// ���������ʽ��֤������
    /// </summary>
    public class ValidateUtil
    {
        private static Regex RegNumber = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //�ȼ���^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w Ӣ����ĸ�����ֵ��ַ������� [a-zA-Z0-9] �﷨һ�� 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region �û��������ʽ

        /// <summary>
        /// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
        /// </summary>
        /// <returns>�ַ�����</returns>
        public static int GetStringLength(string stringValue)
        {
            return Encoding.Default.GetBytes(stringValue).Length;
        }

        /// <summary>
        /// ����û�����ʽ�Ƿ���Ч
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            int userNameLength = GetStringLength(userName);
            if (userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$"))
            {   // �ж��û����ĳ��ȣ�4-20���ַ��������ݣ�ֻ���Ǻ��֡���ĸ���»��ߡ����֣��Ƿ�Ϸ�
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ������Ч��
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        #endregion

        #region �����ַ������

        /// <summary>
        /// �ж϶����Ƿ�ΪInt��
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:�ǣ�fasel:��</returns>
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
        /// �ж϶����Ƿ�ΪLong��
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:�ǣ�fasel:��</returns>
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
        /// �ж϶����Ƿ�ΪDouble��
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:�ǣ�fasel:��</returns>
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
        /// �ж϶����Ƿ�ΪDecimal��
        /// </summary>
        /// <param name="src"></param>
        /// <returns>true:�ǣ�fasel:��</returns>
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
        /// �ж��ַ����Ƿ�Ϊ�մ���null
        /// </summary>
        /// <param name="src"></param>
        /// <returns>����ַ���Ϊ�մ�����null�򷵻�true</returns>
        public static bool IsBlank(String src)
        {
            if (src == null)
                return true;
            if (src.Trim().Length == 0)
                return true;
            return false;
        }


        /// <summary>
        /// ���ն���ת��Ϊָ���ַ���
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

        #region ���ļ��

        /// <summary>
        /// ����Ƿ��������ַ�
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary> 
        /// ��⺬�������ַ�����ʵ�ʳ��� 
        /// </summary> 
        /// <param name="str">�ַ���</param> 
        public static int GetCHZNLength(string inputData)
        {
            System.Text.ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] bytes = n.GetBytes(inputData);

            int length = 0; // l Ϊ�ַ���֮ʵ�ʳ��� 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //�ж��Ƿ�Ϊ���ֻ�ȫ�ŷ��� 
                {
                    length++;
                }
                length++;
            }
            return length;

        }

        #endregion

        #region ���ø�ʽ

        /// <summary>
        /// ��֤���֤�Ƿ�Ϸ�  15 ��  18λ����
        /// </summary>
        /// <param name="idCard">Ҫ��֤�����֤</param>
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
        /// �Ƿ����ʼ���ַ
        /// </summary>
        /// <param name="inputData">�����ַ���</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// �ʱ���Ч��
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
        /// �̶��绰��Ч��
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
        /// �ֻ���Ч��
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
        /// �绰��Ч�ԣ��̻����ֻ� ��
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
        /// Url��Ч��
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP��Ч��
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain ��Ч��
        /// </summary>
        /// <param name="host">����</param>
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
        /// �ж��Ƿ�Ϊbase64�ַ���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// ��֤�ַ����Ƿ���GUID
        /// </summary>
        /// <param name="guid">�ַ���</param>
        /// <returns></returns>
        public static bool IsGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }








        #endregion

        #region ���ڼ��

        /// <summary>
        /// �ж�������ַ��Ƿ�Ϊ����
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
        /// �ж�������ַ��Ƿ�Ϊ����,��2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        #region ���SQL�ַ����Ƿ�Ϸ�

        /// <summary>
        /// ���SQL�ַ����Ƿ�Ϸ�
        /// </summary>
        /// <param name="sqlInput">�����ַ���</param>
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

        #region ����

        /// <summary>
        /// ����ַ�����󳤶ȣ�����ָ�����ȵĴ�
        /// </summary>
        /// <param name="sqlInput">�����ַ���</param>
        /// <param name="maxLength">��󳤶�</param>
        /// <returns></returns>			
        public static string CheckMathLength(string inputData, int maxLength)
        {
            if (inputData != null && inputData != string.Empty)
            {
                inputData = inputData.Trim();
                if (inputData.Length > maxLength)//����󳤶Ƚ�ȡ�ַ���
                {
                    inputData = inputData.Substring(0, maxLength);
                }
            }
            return inputData;
        }        

        #endregion

        #region ����Σ��HTML����

        /// �˴�����Σ��HTML����
        /// </summary>
        /// <param name="html">Ҫ���˵�html�ַ���</param>
        /// <returns></returns>
        public static string FilterHTML(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";

            #region ���� script
            System.Text.RegularExpressions.Regex regex_script1 = new System.Text.RegularExpressions.Regex("(<script[\\s\\S]*?\\/script\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_script2 = new System.Text.RegularExpressions.Regex("(<(script[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_script1.Replace(html, "");
            html = regex_script1.Replace(html, "");
            #endregion ���� script


            #region ���� <iframe> ��ǩ
            System.Text.RegularExpressions.Regex regex_iframe1 = new System.Text.RegularExpressions.Regex("(<iframe [\\s\\S]*?\\/iframe\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_iframe2 = new System.Text.RegularExpressions.Regex("(<(iframe [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_iframe1.Replace(html, "");
            html = regex_iframe2.Replace(html, "");
            #endregion ���� <iframe> ��ǩ

            #region ���� <frameset> ��ǩ
            System.Text.RegularExpressions.Regex regex_frameset1 = new System.Text.RegularExpressions.Regex("(<frameset [\\s\\S]*?\\/frameset\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frameset2 = new System.Text.RegularExpressions.Regex("(<(frameset [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frameset1.Replace(html, "");
            html = regex_frameset2.Replace(html, "");
            #endregion ���� <frameset> ��ǩ

            #region ���� <frame> ��ǩ
            System.Text.RegularExpressions.Regex regex_frame1 = new System.Text.RegularExpressions.Regex("(<frame[\\s\\S]*?\\/frame\\s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frame2 = new System.Text.RegularExpressions.Regex("(<(frame[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frame1.Replace(html, "");
            html = regex_frame2.Replace(html, "");
            #endregion ���� <frameset> ��ǩ

            #region ���� <form> ��ǩ
            System.Text.RegularExpressions.Regex regex_form1 = new System.Text.RegularExpressions.Regex("(<(form [\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_form2 = new System.Text.RegularExpressions.Regex("(<(/form[\\s\\S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_form1.Replace(html, "");
            html = regex_form2.Replace(html, "");
            #endregion ���� <form> ��ǩ

            #region ���� on: ���¼�

            //����on �������ŵ� ����on  ��˫���ŵ� ����on ���������ŵ�
            html = GetReplace(html, "<[\\s\\S]+ (on)[a-zA-Z]{4,20} *= *[\\S ]{3,}>",
                "((on)[a-zA-Z]{4,20} *= *'[^']{3,}')|((on)[a-zA-Z]{4,20} *= *\"[^\"]{3,}\")|((on)[a-zA-Z]{4,20} *= *[^>/ ]{3,})", "");

            #endregion ���� on: ���¼�

            #region ���� javascript: ���¼�

            html = GetReplace(html, "<[\\s\\S]+ (href|src|background|url|dynsrc|expression|codebase) *= *[ \"\']? *(javascript:)[\\S]{1,}>"
                , "(' *(javascript|vbscript):([\\S^'])*')|(\" *(javascript|vbscript):[\\S^\"]*\")|([^=]*(javascript|vbscript):[^/> ]*)", "#");
            #endregion ���� javascript: ���¼�

            return html;
        }


        /// <summary>
        /// ����˫�ع���
        /// </summary>
        /// <param name="content"></param>
        /// <param name="splitKey1"></param>
        /// <param name="splitKey2"></param>
        /// <param name="newChars"></param>
        /// <returns></returns>
        public static string GetReplace(string content, string splitKey1, string splitKey2, string newChars)
        {
            //splitKey1 ��һ������ʽƥ��  

            //splitKey2 ƥ�������ٴ�ƥ������滻  

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
