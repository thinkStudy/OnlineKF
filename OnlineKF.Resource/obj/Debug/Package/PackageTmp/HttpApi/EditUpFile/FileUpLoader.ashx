<%@ WebHandler Language="C#" Class="imageUp" %>


using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using OnlineKF.Resource;
using OnlineKF.Utils;

public class imageUp : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8; 
       
        //上传配置
        try
        {   
            string callback = context.Request["callback"];
            string toUrl = context.Request["toUrl"];
            string fileId = context.Request["fileId"];
            string pathbase = "OnlieKFile/";                                                          //保存路径
            int size = int.Parse(ConfigHelper.GetConfigString("fileSize"));                     //文件大小限制,单位mb                                                                                   //文件大小限制，单位KB
           
            //上传图片
            Hashtable info;
            Uploader up = new Uploader();

            info = up.upFile(context, pathbase, size); //获取上传状态


            string json = BuildJson(info);

            context.Response.ContentType = "text/html";

            if (toUrl != null)
            {

                ReturnParamsUtil.CommonUploadWrite(context.Response, toUrl, callback, fileId, info["url"].ToString(), info["originalName"].ToString(), info["state"].ToString(), info["imgInfo"].ToString());

            }
            else
            {
                context.Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            LogUtils.LogWriterToIISOut(ex);
            context.Response.ContentType = "text/html";
            context.Response.Write("");
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string BuildJson(Hashtable info)
    {
        List<string> fields = new List<string>();
        string[] keys = new string[] { "originalName", "name", "url", "size", "state", "type", "imgInfo" };
        for (int i = 0; i < keys.Length; i++)
        {
            fields.Add(String.Format("\"{0}\": \"{1}\"", keys[i], info[keys[i]]));
        }
        return "{" + String.Join(",", fields) + "}";
    }

    private string BuildToString(Hashtable info)
    {
        List<string> fields = new List<string>();
        string[] keys = new string[] { "originalName", "name", "url", "size", "state", "type", "imgInfo" };
        for (int i = 0; i < keys.Length; i++)
        {
            fields.Add(String.Format("{0}|{1}", keys[i], info[keys[i]]));
        }
        return String.Join("$", fields);
    }

}