using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.AjaxHander.HttpHandler;
using OnlineKF.Model;
using OnlineKF.CommonUtil;
using OnlineKF.BLL;
using OnlineKF.Utils;

namespace OnlineKF.AjaxHander
{
    public class MessageDataAjax
    {
        /// <summary>
        /// 添加聊天记录
        /// </summary>
        [Ajax(true)]
        public ResultModel newQuestion(string questionId,string content,int type,string time,bool isFirstAdd,int addType)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                var msgType = addType == 0 ? "qm" : "sm";

                MessageDataModel model = new MessageDataModel();
                model.message = "{" + msgType + ":{m:'" + content + "',t:" + type + ",d:" + time + "}}";
                model.questionid = questionId;
                model.onlycount = 1;
                if (isFirstAdd)
                {
                    model.createtime = DateTime.Now;
                    var addResult = MessageDataBLL.GetInstance().Add(model);
                    data = "{ id:" + addResult + " }";
                }
                else {
                    var maxLength = int.Parse(ConfigHelper.GetConfigString("maxMessageLength"));
                    model.message = "{" + msgType + ":{m:'" + content + "',t:" + type + ",d:" + time + "}}";
                    MessageDataBLL.GetInstance().totalInsert(model, maxLength);
                   
                }
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
        /// 咨询人员查询聊天记录
        /// </summary>
        [Ajax(true)]
        public ResultModel queryQuestionPerson(int msgid)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                MessageDataModel model = MessageDataBLL.GetInstance().GetMessageDataModel(msgid);
                if (model != null )
                {
                    result = true;
                    data = JavaScriptConvert.SerializeObject(model);
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
        /// 客服人员查询聊天记录
        /// </summary>
        [Ajax(true)]
        public ResultModel queryServicePersonMsg(int msgid)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                MessageDataModel model = MessageDataBLL.GetInstance().GetServicePersonMsg(msgid);
                if (model != null)
                {
                    result = true;
                    data = JavaScriptConvert.SerializeObject(model);
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
        /// 客服人员查询未读数量
        /// </summary>
        [Ajax(true)]
        public ResultModel servicePersonOnlyCount()
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
               var serviceId =  UserUtil.getUserModel.id;
               var resultList = MessageDataBLL.GetInstance().queryServicePersonMsg(serviceId);

               data = JavaScriptConvert.SerializeObject(resultList);

               result = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg, data);
        }

        


    }
}