using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;

namespace OnlineKF.Resource
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }
       
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //本代码的功能是检查页面请求的大小，如果超过了配置文件maxRequestLength的设定值，就提示用户超过了所允许的文件大小。

            //从配置文件里得到配置的允许上传的文件大小
            HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");

            //maxRequestLength 为整个页面的大小，不仅仅是上传文件的大小，所以扣除 100KB 的大小，
            //maxRequestLength单位为KB
            int maxRequestLength = (runTime.MaxRequestLength) * 1024;

            //当前请求上下文的HttpApplication实例
            //HttpContext context = ((HttpApplication)sender).Context;

            //判断请求的内容长度是否超过了设置的字节数
            if (Request.ContentLength > maxRequestLength)
            {
                #region 不理解这些代码存在的意义
                /*
                //得到服务对象
                IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

                //检查请求是否包含正文数据
                if (workerRequest.HasEntityBody())
                {
                    //请求正文数据的长度
                    int requestLength = workerRequest.GetTotalEntityBodyLength();
                    //得到加载的初始字节数
                    int initialBytes = 0;
                    if (workerRequest.GetPreloadedEntityBody() != null)
                        initialBytes = workerRequest.GetPreloadedEntityBody().Length;

                    //检查是否所有请求数据可用
                    if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                    {
                        byte[] buffer = new byte[512000];
                        //设置要接收的字节数为初始字节数
                        int receivedBytes = initialBytes;
                        //读取数据，并把所有读取的字节数加起来，判断总的大小
                        while (requestLength - receivedBytes >= initialBytes)
                        {
                            //读取下一块字节
                            initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
                            //更新接收到的字节数
                            receivedBytes += initialBytes;
                        }
                        initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                    }
                }
                */
                #endregion
                //注意这里可以跳转，可以直接终止；在VS里调试时候得不到想要的结果，通过IIS才能得到想要的结果；FW4.0经典或集成都没问题
                
                try
                {
                    ReturnParamsUtil.CommonUploadWrite(HttpContext.Current.Response, Request.QueryString["toUrl"], Request.QueryString["callback"], Request.QueryString["fileId"], "", "", "文件过大，上传限制"+(maxRequestLength/1024)+"M", "");
                    HttpContext.Current.Response.End();
                }
                catch (Exception) { }
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}