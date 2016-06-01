using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using OnlineKF.Model;

namespace OnlineKF.CommonUtil
{
    public static class ReturnDataUtil
    {

        public static string returnPageData(int page,int pageSize,int totalCount,object data,string msg = "")
        {
            var returnStr = "";
            string dataStr = "";
            try
            {
               dataStr = new JavaScriptSerializer().Serialize(data);
                    
            }
            catch (Exception ex) {
                msg = ex.Message;
            }
            returnStr = "{\"page\":" + page + ",\"pageSize\":" + pageSize + ",\"totalCount\":" + totalCount + ",\"msg\":\"" + page + "\",\"data\":" + dataStr + "}";

            return returnStr;
        }
     
        public static string getReturnData(bool result, string msg = "", object data = null)
        {
            var returnStr = "";
            int status = result == true ? 200 : 500;
            var dataStr = "[]";
            try
            {
                if (data != null)
                {
                    dataStr = new JavaScriptSerializer().Serialize(data);
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            returnStr = "{\"status\":" + status + ",\"msg\":\"" + msg + "\",\"data\":" + dataStr + "}";

            return returnStr;
        }

        public static ResultPageModel returnPageModel(bool success, int page, int pageSize, int totalCount, object data, string msg = "")
        {

            ResultPageModel result = new ResultPageModel();
            result.status = success == true ? 200:500;
            result.msg = msg;
            result.data = data;
            result.page = page;
            result.pageSize = pageSize;
            result.totalCount = totalCount;

            return result;
        }

        public static ResultModel getReturnModel(bool success, string msg = "", object data = null)
        {
            int status = success == true ? 200 : 500;

            ResultModel result = new ResultModel();
            result.status = status;
            result.msg = msg;
            result.data = data;

            return result;
        }
    }
}