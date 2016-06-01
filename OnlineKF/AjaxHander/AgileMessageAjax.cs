using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.AjaxHander.HttpHandler;
using OnlineKF.Model;
using OnlineKF.BLL;
using OnlineKF.Utils;
using OnlineKF.CommonUtil;

namespace OnlineKF.AjaxHander
{
    public class AgileMessageAjax
    {
        /// <summary>
        /// 查询快捷回复信息
        /// </summary>
        [Ajax(true)]
        public ResultModel queryAgileMessage()
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                int totalCount = 0;
                var userModel = UserUtil.getUserModel;
                var modelList = AgileReplyBLL.GetInstance().GetAgileReplyList(100, 1, " and company = "+userModel.compayid+" ", out totalCount, " order by usenumber,createdate desc ");
               
                data = JavaScriptConvert.SerializeObject(modelList);
                result = true;
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg, data);
        }

        /// <summary>
        /// 添加快捷回复信息
        /// </summary>
        [Ajax(true)]
        public ResultModel addAgileMessage(string agileMsg)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                var userModel = UserUtil.getUserModel;

                AgileReplyModel model = new AgileReplyModel();
                model.company = userModel.compayid;
                model.hasglobal = true;
                model.messagetxt = agileMsg;
                model.serviceId = userModel.id;
                model.usenumber = 0;
                model.createdate = DateTime.Now;

                var addId =  AgileReplyBLL.GetInstance().Add(model);
                if (addId > 0)
                {
                    data = "{ \"id\":" + addId + " }";
                    result = true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg, data);
        }

        /// <summary>
        /// 删除快捷回复信息
        /// </summary>
        [Ajax(true)]
        public ResultModel delAgileMessage(int id)
        {
            var msg = "";
            var result = false;
            try
            {
                result = AgileReplyBLL.GetInstance().Delete(id);
               
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg);
        }

        /// <summary>
        /// 记录使用次数
        /// </summary>
        [Ajax(true)]
        public ResultModel logUseNumber(int id, int usenumber)
        {
            var msg = "";
            var result = false;
            try
            {
                AgileReplyModel model = new AgileReplyModel();
                model.id = id;
                model.usenumber = usenumber;
                model.createdate = DateTime.Now;
                result = AgileReplyBLL.GetInstance().logUseNumber(model);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg);
        }
    }
}