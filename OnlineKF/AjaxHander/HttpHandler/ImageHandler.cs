using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;

namespace OnlineKF.AjaxHander.HttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    class ImageHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            
            string url = context.Request.Url.PathAndQuery;

            //string LocalResourcesPath = ConfigurationManager.AppSettings["LocalResources"];
            //if (url.IndexOf(LocalResourcesPath) == -1)
            //{
            //    url = url.Replace("/LocalResources/zh-CN", LocalResourcesPath);
            //}
            context.Response.ContentType = "text/html";//image/JPEG
            try
            {
                context.Response.WriteFile(url);
            }
            catch { }
        }
    }
}
