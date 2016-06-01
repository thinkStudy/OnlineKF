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
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;

namespace OnlineKF.Utils
{
    /// <summary>
    /// HttpHelper 的摘要说明
    /// Http提交的辅助类
    /// </summary>
    public class HttpHelper
    {
        public HttpHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public const int Timeout = 20000;//20秒

        /// <summary>
        /// 根据参数列表生成查询字符串
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static String GenQueryString(List<KeyValuePair<String, String>> parameters, Encoding encoding)
        {
            if (parameters == null || parameters.Count < 1)
            {
                return "";
            }
            StringBuilder bodyBuilder = new StringBuilder();
            foreach (KeyValuePair<String, String> item in parameters)
            {
                String name = item.Key;
                String val = HttpUtility.UrlEncode(item.Value, encoding);
                bodyBuilder.Append(string.Format("{0}={1}&", name, val));
            }
            String result = bodyBuilder.ToString().TrimEnd('&');
            return result;
        }

        /// <summary>
        /// 用get方法访问指定的URl，返回获取的http内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="responseCode"></param>
        /// <returns></returns>
        public static String HttpGet(String url, List<KeyValuePair<String, String>> parameters, Encoding encoding, out HttpStatusCode responseCode)
        {
            return HttpGet(url, parameters, null, encoding, out responseCode);
        }

        /// <summary>
        /// 用get方法访问指定的URl，返回获取的http内容
        /// </summary>
        /// <param name="url">要调用的url</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="headers">header</param>
        /// <param name="encoding">发生请求及解析响应内容的编码</param>
        /// <param name="responseCode">页面响应的状态码，如200</param>
        /// <returns></returns>
        public static String HttpGet(String url, List<KeyValuePair<String, String>> parameters, List<KeyValuePair<String, String>> headers, Encoding encoding, out HttpStatusCode responseCode)
        {
            String paras = GenQueryString(parameters, encoding);
            String conStr = "?";
            if (url.IndexOf("?") >= 0)
                conStr = "&";
            url = url + conStr + paras;
            HttpWebRequest httpWebRequest = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                //httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    httpWebRequest.Headers.Add(headers[i].Key, headers[i].Value);
                }
            }

            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = Timeout;//10秒         

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException wex)
            {
                response = (HttpWebResponse)wex.Response;
                if (response == null)
                    throw wex;
            }

            responseCode = response.StatusCode;
            String content = "";
            //if (response.ContentLength > 0)//有些非标准的接口不一定会返回这个属性
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                content = reader.ReadToEnd();
                responseStream.Close();
            }
            response.Close();
            httpWebRequest.Abort();
            return content;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        /// <summary>
        /// 用post方法访问指定的URl，返回获取的http内容
        /// </summary>
        /// <param name="url">要调用的url</param>
        /// <param name="parameters">参数，形如p1=v1&p2=v2</param>
        /// <param name="encoding">发生请求及解析响应内容的编码</param>
        /// <param name="responseCode">页面响应的状态码，如200</param>
        /// <returns></returns>
        public static String HttpPost(String url, List<KeyValuePair<String, String>> parameters, Encoding encoding, out HttpStatusCode responseCode)
        {
            return HttpPost(url, parameters, null, encoding, out responseCode);
        }

        /// <summary>
        /// 用post方法访问指定的URl，返回获取的http内容
        /// </summary>
        /// <param name="url">要调用的url</param>
        /// <param name="parameters">参数，形如p1=v1&p2=v2</param>
        /// <param name="headers">header</param>
        /// <param name="encoding">发生请求及解析响应内容的编码</param>
        /// <param name="responseCode">页面响应的状态码，如200</param>
        /// <returns></returns>
        public static String HttpPost(String url, List<KeyValuePair<String, String>> parameters, List<KeyValuePair<String, String>> headers, Encoding encoding, out HttpStatusCode responseCode)
        {
            HttpWebRequest httpWebRequest = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                //httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    httpWebRequest.Headers.Add(headers[i].Key, headers[i].Value);
                }
            }

            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = Timeout;//10秒
            String postData = GenQueryString(parameters, encoding);

            byte[] bytes = encoding.GetBytes(postData);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = bytes.Length;

            Stream writeStream = httpWebRequest.GetRequestStream();
            writeStream.Write(bytes, 0, bytes.Length);
            writeStream.Close();

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException wex)
            {
                response = (HttpWebResponse)wex.Response;
                if (response == null)
                    throw wex;
            }

            responseCode = response.StatusCode;

            String content = "";
            //if (response.ContentLength > 0)//有些非标准的接口不一定会返回这个属性
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                content = reader.ReadToEnd();
                responseStream.Close();
            }
            response.Close();
            httpWebRequest.Abort();

            return content;
        }

    }
}

