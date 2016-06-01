using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.Utils;

namespace OnlineKF.Resource
{
    public static class ReturnParamsUtil
    {

       
        public static void CommonUploadWrite(HttpResponse Response, string toUrl, string callBack,string fileId, string savePath, string name, string state, string imgInfo)
        {
            //info["url"], info["name"], info["state"], info["imgInfo"]);
            string retStr = "<script>window.location.href=\"{0}?savePath={1}&name={2}&state={3}&imgInfo={4}&fileId={5}\"</script>";
            savePath = HttpUtility.UrlEncode(savePath);
            state = HttpUtility.UrlEncode(state);
            if (!string.IsNullOrEmpty(callBack))
            {
                //有回調函數
                retStr = "<script>window.location.href=\"{0}?savePath={1}&name={2}&state={3}&imgInfo={4}&fileId={5}&callBack={6}\"</script>";
                retStr = string.Format(retStr, toUrl, savePath, name, state, imgInfo,fileId, callBack);
            }
            else
            {
                retStr = string.Format(retStr, toUrl, savePath, name, state, imgInfo,fileId, callBack);
            }
            Response.Write(retStr);
        }

        //删除文件成功调用方法
        public static void FileDeleteWrite(HttpResponse Response, string toUrl, string callBack, bool status)
        {

            string retStr = "<script>window.location.href=\"{0}?status={0}\"</script>";

            if (!string.IsNullOrEmpty(callBack))
            {
                //有回調函數
                retStr = "<script>window.location.href=\"{0}?callBack={1}&status={2}\"</script>";
                retStr = string.Format(retStr, toUrl, callBack, status);
            }
            else
            {
                retStr = string.Format(retStr, toUrl, status);
            }
            Response.Write(retStr);
        }
    }
}