using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    public static class LogUtils
    {
        #region 错误日志相关
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static void LogWriter(string info)
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\log";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string file = string.Format("{0}\\{1:yyyyMMdd}.log", path, DateTime.Now);
                using (FileStream fs = File.Open(file, FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine(info);
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 往iis外面写错误日志，路径配置于web.config 的 LogPath
        /// </summary>
        /// <param name="ex">错误信息</param>
        public static void LogWriterToIISOut(string msg)
        {
            //输出错误信息
            LogWriterToIISOut(new Exception(msg), "", "");
        }
        /// <summary>
        /// 往iis外面写错误日志，路径配置于web.config 的 LogPath
        /// </summary>
        /// <param name="ex">错误信息</param>
        public static void LogWriterToIISOut(Exception ex)
        {
            //输出错误信息
            LogWriterToIISOut(ex, "", "");
        }

        /// <summary>
        /// 往iis外面写错误日志，路径配置于web.config 的 LogPath
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="uid">操作人id</param>
        /// <param name="uName">操作人名称</param>
        public static void LogWriterToIISOut(Exception ex, string uId, string uName)
        {
            // 取当前用户信息
            string extraInformation = string.Empty;
            if (uId != string.Empty && uName != string.Empty)
            {
                extraInformation = string.Format("UserId:{0} UserName:{1}", uId, uName);
            }

            // 如果不是文件不存在错误则记录
            if (ex.Message.IndexOf("不存在") == -1 && ex.Message.IndexOf("无法连接到") == -1)
            {
                WriteEntry(ex, extraInformation);
            }
        }

        /// <summary>
        /// 记录异常日志，使用默认的日志存放文件夹。
        /// </summary>
        /// <param name="exception">异常对象</param>
        public static void WriteEntry(Exception exception)
        {
            WriteEntry(exception, null, ConfigHelper.GetLogPath());
        }

        /// <summary>
        /// 记录异常日志，使用默认的日志存放文件夹。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="extraInformation">其它额外信息</param>
        public static void WriteEntry(Exception exception, string extraInformation)
        {
            WriteEntry(exception, extraInformation, ConfigHelper.GetLogPath());
        }

        /// <summary>
        /// 记录异常日志。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="extraInformation">其它额外信息</param>
        /// <param name="logPath">日志存放文件夹</param>
        public static void WriteEntry(Exception exception, string extraInformation, string logFolder)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                string filePath = logFolder + "\\" + fileName;

                // 确保文件夹存在
                if (!Directory.Exists(logFolder))
                {
                    System.IO.Directory.CreateDirectory(logFolder);
                }

                // 确保文件存在
                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.WriteLine("Fields           :Value");
                        sw.WriteLine();
                    }
                }

                // 确保还没有记录
                bool logExists = false;
                //using (StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                //{
                //    string log = sr.ReadToEnd();
                //    logExists = log.IndexOf(LogUtils.RawUrl) > -1 && log.IndexOf(exception.Message) > -1;
                //}

                if (!logExists)
                {
                    // 开始写入日志
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine("===========================================");
                        sw.WriteLine("DateTime         : " + LogUtils.GetDateTime());
                        sw.WriteLine("HttpReferrer     : " + LogUtils.UrlReferrer);
                        sw.WriteLine();
                        sw.WriteLine("HttpPathAndQuery : " + LogUtils.RawUrl);
                        sw.WriteLine("Message          : " + exception.Message);
                        sw.WriteLine("StackTrace       : " + exception.StackTrace.Substring(3).Replace("   ", "                   "));
                        if (!string.IsNullOrEmpty(extraInformation))
                        {
                            sw.WriteLine("-------------------------------------------");
                            sw.WriteLine("ExtraInformation : " + extraInformation);
                        }
                        sw.WriteLine();
                    }
                }

            }
            catch { }
        }

        #endregion

        #region 地址相关
        /// <summary>
        /// 获取 当前请求完整Url地址(包括参数)。
        /// </summary>
        public static string Url
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.Url.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取 当前请求的原始URL(URL中域信息之后的部分,包括参数)。
        /// </summary>
        public static string RawUrl
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.RawUrl;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取 有关客户端上次请求的URL的信息(包括参数)，该请求链接到当前的URL。
        /// </summary>
        public static string UrlReferrer
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.UrlReferrer.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }


        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回标准时间 
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {

                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

      

        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string mashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }
    }
}
