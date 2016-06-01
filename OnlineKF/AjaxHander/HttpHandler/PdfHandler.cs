using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using OnlineKF.Utils;
using System.Configuration;
using System.Web.SessionState;

namespace OnlineKF.AjaxHander.HttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    class PdfHandler : IHttpHandler, IReadOnlySessionState 
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {

            string url = context.Request.PhysicalPath;

            //string LocalResourcesPath = ConfigurationManager.AppSettings["LocalResources"];
            //if (url.IndexOf(LocalResourcesPath) == -1)
            //{
            //    url = url.Replace("/LocalResources/zh-CN", LocalResourcesPath);
            //}
            LogUtils.LogWriterToIISOut("------------test___pdf -----------");
            context.Response.ContentType = "application/pdf";//image/JPEG
            try
            {
                context.Response.WriteFile(url);
            }
            catch { }
        }
    }
}
