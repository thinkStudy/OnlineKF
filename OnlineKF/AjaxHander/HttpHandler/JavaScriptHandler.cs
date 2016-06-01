using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using OnlineKF.Utils;

namespace OnlineKF.AjaxHander.HttpHandler
{
    /// <summary>
    /// 对请求的js文件进行压缩和混淆，然后按照设定进行语言化
    /// </summary>
    class JavaScriptHandler : IHttpHandler, IReadOnlySessionState 
    {
        public void ProcessRequest(HttpContext context)
        {

            #region 配置压缩js的信息
            ECMAScriptPacker p = new ECMAScriptPacker();

            if (System.Configuration.ConfigurationManager.GetSection("ecmascriptpacker") != null)
            {
                NameValueCollection cfg = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("ecmascriptpacker");
                if (cfg["Encoding"] != null)
                {
                    switch (cfg["Encoding"].ToLower())
                    {
                        case "none":
                            p.Encoding = ECMAScriptPacker.PackerEncoding.None;
                            break;
                        case "numeric":
                            p.Encoding = ECMAScriptPacker.PackerEncoding.Numeric;
                            break;
                        case "mid":
                            p.Encoding = ECMAScriptPacker.PackerEncoding.Mid;
                            break;
                        case "normal":
                            p.Encoding = ECMAScriptPacker.PackerEncoding.Normal;
                            break;
                        case "highascii":
                        case "high":
                            p.Encoding = ECMAScriptPacker.PackerEncoding.HighAscii;
                            break;
                    }
                }
                if (cfg["FastDecode"] != null)
                {
                    if (cfg["FastDecode"].ToLower() == "true")
                        p.FastDecode = true;
                    else
                        p.FastDecode = false;
                }
                if (cfg["SpecialChars"] != null)
                {
                    if (cfg["SpecialChars"].ToLower() == "true")
                        p.SpecialChars = true;
                    else
                        p.SpecialChars = false;
                }
                if (cfg["Enabled"] != null)
                {
                    if (cfg["Enabled"].ToLower() == "true")
                        p.Enabled = true;
                    else
                        p.Enabled = false;
                }
            }
            
            // try and read settings from URL
            if (context.Request.QueryString["Encoding"] != null)
            {
                switch (context.Request.QueryString["Encoding"].ToLower())
                {
                    case "none":
                        p.Encoding = ECMAScriptPacker.PackerEncoding.None;
                        break;
                    case "numeric":
                        p.Encoding = ECMAScriptPacker.PackerEncoding.Numeric;
                        break;
                    case "mid":
                        p.Encoding = ECMAScriptPacker.PackerEncoding.Mid;
                        break;
                    case "normal":
                        p.Encoding = ECMAScriptPacker.PackerEncoding.Normal;
                        break;
                    case "highascii":
                    case "high":
                        p.Encoding = ECMAScriptPacker.PackerEncoding.HighAscii;
                        break;
                }
            }
            if (context.Request.QueryString["FastDecode"] != null)
            {
                if (context.Request.QueryString["FastDecode"].ToLower() == "true")
                    p.FastDecode = true;
                else
                    p.FastDecode = false;
            }
            if (context.Request.QueryString["SpecialChars"] != null)
            {
                if (context.Request.QueryString["SpecialChars"].ToLower() == "true")
                    p.SpecialChars = true;
                else
                    p.SpecialChars = false;
            }
            if (context.Request.QueryString["Enabled"] != null)
            {
                if (context.Request.QueryString["Enabled"].ToLower() == "true")
                    p.Enabled = true;
                else
                    p.Enabled = false;
            }
            //handle the request
            #endregion

            var readyPath = context.Request.PhysicalPath;
            //判断是否使用压缩文件
            if (readyPath.IndexOf(".min.") ==-1 && IsReusable )
                readyPath = context.Request.PhysicalPath.Replace(".js", ".min.js");

            TextReader r = new StreamReader(readyPath);
            string jscontent = r.ReadToEnd();
            r.Close();
           
            context.Response.ContentType = "text/javascript";
            
            //if(IsReusable)
            //    context.Response.Output.Write(p.Pack(jscontent));
            //else
                context.Response.Output.Write(jscontent);
           
            /*
            TextReader r = new StreamReader(context.Request.PhysicalPath);
            string jscontent = r.ReadToEnd();
            r.Close();

            string[] sl = Regex.Split(jscontent, "(#<[^#]+>#)");
            for (int i = 0; i < sl.Length; i++)
            {
                if (sl[i].StartsWith("#<"))
                {
                    try
                    {
                        if (Current.GetLocalResources[sl[i].Trim(new char[] { '#', '>', '<' })].IndexOf("'") > 0)
                        {
                            sl[i] = Current.GetLocalResources[sl[i].Trim(new char[] { '#', '>', '<' })].Replace("'", "’");
                        }
                        else
                        {
                            sl[i] = Current.GetLocalResources[sl[i].Trim(new char[] { '#', '>', '<' })];
                        }
                    }
                    catch { }
                }
            }

            context.Response.ContentType = "text/javascript";
            context.Response.Output.Write(string.Join("", sl));
             */
        }

        //获取配置是否压缩js
        public bool IsReusable
        {
            get
            {
                if (ConfigHelper.GetConfigString("isJsMinifiler") != "")
                {
                    if (ConfigHelper.GetConfigString("isJsMinifiler").ToLower() == "true")
                       return true;
                }
                return false;
            }
        }
    }
}
