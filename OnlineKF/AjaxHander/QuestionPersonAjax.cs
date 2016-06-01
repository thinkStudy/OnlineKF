using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.AjaxHander.HttpHandler;
using OnlineKF.Model;
using OnlineKF.Utils;
using OnlineKF.CommonUtil;
using OnlineKF.BLL;

namespace OnlineKF.AjaxHander
{
    public class QuestionPersonAjax
    {
        /// <summary>
        /// 创建咨询订单
        /// </summary>
        [Ajax(true)]
        public ResultModel newQuestion(int companyId)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                QuestionPersonModel model = UserUtil.getQuestionPersonModel; 
                if (model == null)
                {
                    model = new QuestionPersonModel();
                    model.createTime = DateTime.Now;
                    model.id = System.Guid.NewGuid().ToString();
                    model.serviceid = AutoServiceUtil.getOnlinePerson(companyId);
                    model.status = 1;
                    if (model.serviceid > 0)
                    {
                        result = QuestionPersonBLL.GetInstance().Add(model);
                        data = "{ \"questionId\":\"" + model.id + "\" }";
                        if(result)
                            UserUtil.saveQuestInfo(model);
                    }
                    else
                    {
                        msg = "当前没有在线客服";
                    }
                    
                }
                else {
                    result = true;
                    var msglist = MessageDataBLL.GetInstance().getModelList(" AND questionid = '"+model.id+"'");
                    var msgId = 0;
                    if (msglist.Count > 0) {
                        msgId = msglist[0].id;
                    }
                    data = "{ \"questionId\":\"" + model.id + "\",msgId:" + msgId + " }";
                }
               
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg,data);
        }

        /// <summary>
        /// 关闭咨询订单
        /// </summary>
        [Ajax(true)]
        public ResultModel closeQuestion(string questionId)
        {
            var msg = "";
            var data = "";
            var result = false;
            try
            {
                UserUtil.removeQuestion();
                QuestionPersonBLL.GetInstance().closeQuestion(SQLHelper.checkSqlString(questionId));
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
        /// 给客服人员评分
        /// </summary>
        [Ajax(true)]
        public ResultModel setServiceLevel(string questionId,int serviceLevel)
        {
            var msg = "";
            var result = false;
            try
            {
                QuestionPersonModel model = new QuestionPersonModel();
                model.id = SQLHelper.checkSqlString(questionId);
                model.servicelevel = serviceLevel;
                result = QuestionPersonBLL.GetInstance().setServiceLevel(model);
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