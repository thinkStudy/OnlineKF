using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for GlobalHelper
/// </summary>
namespace OnlineKF.Utils
{
    public class WebHelper
    {
        public WebHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// 获取当前web应用的完整根路径http://xxx.xxx/xx，去掉了路径中最后一个"/"
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string GetFullAppPath(HttpRequest Request)
        {
            String path = Request.Url.Scheme + "://" + Request.Url.Host
                + (Request.Url.Port == 80 ? "" : (":" + Request.Url.Port)) + Request.ApplicationPath;
            if (path.EndsWith("/"))
            {
                path = path.Remove(path.Length - 1);
            }
            return path;
        }

        /// <summary>
        /// 获取当前web应用的根路径，去掉了路径中最后一个"/"
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string GetAppPath(HttpRequest Request)
        {
            String path = Request.ApplicationPath;
            if (path.EndsWith("/"))
            {
                path = path.Remove(path.Length - 1);
            }
            return path;
        }
    }
}