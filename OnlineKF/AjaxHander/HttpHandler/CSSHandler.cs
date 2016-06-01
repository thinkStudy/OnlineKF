using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.IO;
using OnlineKF.Utils;


namespace OnlineKF.AjaxHander.HttpHandler
{
    public class CSSHandler : IHttpHandler, IReadOnlySessionState
    {

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// 标识模块的CSS是否压缩
        /// </summary>
        public static bool isCsscompress
        {
            get
            {
                if (ConfigHelper.GetConfigString("isCSSMinifiler") != "")
                {
                    if (ConfigHelper.GetConfigString("isCSSMinifiler").ToLower() == "true")
                        return true;
                }
                return false;
            }
        }

        public static string Compress(HttpContext context, string fcontent)
        {
            if (isCsscompress)
                return CssMinifier.CssMinify(fcontent).ToString();
            else
                return fcontent;
        }

        public void ProcessRequest(HttpContext context)
        {
            var readyPath = context.Request.PhysicalPath;
            //判断是否使用压缩文件
            if (isCsscompress)
                readyPath = context.Request.PhysicalPath.Replace(".css", ".min.css");

            TextReader r = new StreamReader(readyPath);
            string csscontent = r.ReadToEnd();
            r.Close();
            context.Response.ContentType = "text/css";
            context.Response.Output.Write(csscontent);
            context.Response.End();
        }
    }
}