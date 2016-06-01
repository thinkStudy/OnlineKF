using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.Model;
using OnlineKF.BLL;
using System.Web.UI;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using OnlineKF.Utils;

namespace OnlineKF.CommonUtil
{
    public static class CommonUtils
    {

        /// <summary>
        /// 获取文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetFileShowPath()
        {
            try
            {
                return ConfigurationManager.AppSettings["ImagePath"];
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 文件保存的路径
        /// </summary>
        /// <returns></returns>
        public static string GetFileVirtualPath()
        {
            try
            {
                return ConfigurationManager.AppSettings["VirtualPath"];
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static JavaScriptSerializer _singleJSS;
        /// <summary>
        /// 序列化静态类
        /// </summary>
        /// <returns></returns>
        public static JavaScriptSerializer GetJSSerializer()
        {
            if (_singleJSS == null)
            {
                _singleJSS = new JavaScriptSerializer();
            }
            return _singleJSS;
        }
        ///
        /// 1.判断远程文件是否存在 
        ///fileUrl:远程文件路径，包括IP地址以及详细的路径
        ///
        public static bool RemoteFileExists(string fileUrl)
        {
            bool result = false;//下载结果

            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);

                response = req.GetResponse();

                result = response == null ? false : true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }  /// <summary>


        public static void RequestUrlWrite(HttpResponse Response, string toUrl, string callBack, string Uvdata)
        {
            string retStr = "<script>window.location.href=\"{0}?Uvdata={1}\"</script>";
            Uvdata = HttpUtility.UrlEncode(Uvdata);
           
            if (!string.IsNullOrEmpty(callBack))
            {
                //有回調函數
                retStr = "<script>window.location.href=\"{0}?Uvdata={1}&callBack{2}\"</script>";
                retStr = string.Format(retStr, toUrl, Uvdata,callBack);
            }
            else
            {
                retStr = string.Format(retStr, toUrl, Uvdata);
            }
            Response.Write(retStr);
        }


    }
}