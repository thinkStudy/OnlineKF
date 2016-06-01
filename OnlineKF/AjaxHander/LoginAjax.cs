using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.AjaxHander.HttpHandler;
using OnlineKF.Model;
using OnlineKF.CommonUtil;
using OnlineKF.Utils;
using OnlineKF.BLL;

namespace OnlineKF.AjaxHander
{
    public class LoginAjax
    {

        private static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// 客服人员登录
        /// </summary>
        [Ajax(true)]
        public ResultModel loginOnlineKF(string loginName,string loginPwd)
        {
            ResultModel model = new ResultModel();
            var msg = "";
            var result = false;
            try
            {
                ServicePersonModel userModel = new ServicePersonModel();
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))
                    msg = "参数不正确！";
                else
                    userModel = ServicePersonBLL.GetInstance().GetServicePersonModel(loginName);

                var md5Pwd = SecurityHelper.MD5Encrypt(loginPwd);

                if (userModel != null && md5Pwd == userModel.loginpwd)
                {
                    result = true;
                    //存储登录信息
                    UserUtil.saveUserInfo(userModel);
                }
                else {
                    msg = "用户名或者密码错误！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg);
        }

        /// <summary>
        /// 客服人员保存
        /// </summary>
        [Ajax(true)]
        public ResultModel autoSaveOnline()
        {
            ResultModel model = new ResultModel();
            var msg = "";
            var result = true;
            try
            {
                ServicePersonModel userModel = UserUtil.getUserModel;
                if (userModel != null)
                {
                    AutoServiceUtil.addOnline(userModel);
                }
                else {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                msg = ex.Message;
                LogUtils.LogWriterToIISOut(ex);
            }
            return ReturnDataUtil.getReturnModel(result, msg);
        }

        /// <summary>
        /// 客服人员退出
        /// </summary>
        [Ajax(true)]
        public ResultModel loginOut()
        {
            ResultModel model = new ResultModel();
            var msg = "";
            var result = UserUtil.loginOut();
            
            return ReturnDataUtil.getReturnModel(result, msg);
        }
    }
}