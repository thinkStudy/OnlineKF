using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.Utils;
using System.Web.UI;
using OnlineKF.Model;


namespace OnlineKF.CommonUtil
{
    public static class UserUtil
    {
        public static string _Key = "37303542444431342d313843332d343439342d383736382d3138303536384646";
        public static string _IV = "33633392d344336312d3930324444256";
        public static string USER_COOKIE_KEY = "user";
        // 登陆用户 Session Name 
        public static string SESSION_KEY_ACCOUNT = "SESSION:7LE:ACCOUNT";
        // 登陆用户 cookies key 
        public static string COOKIES_KEY_ACCOUNT = "COOKIES:7LE:ACCOUNT";

        // 咨询问题 cookies key 
        public static string COOKIES_KEY_QUESTION = "COOKIES:7LE:QUESTION";

        public static void saveQuestInfo(QuestionPersonModel model) {
            var respose = HttpContext.Current.Response;
            // 存储登录状态
            respose.Cookies.Remove(COOKIES_KEY_QUESTION);
           

            HttpCookie cookie = new HttpCookie(COOKIES_KEY_QUESTION);

            cookie.Path = "/";
           
            cookie.Value = JavaScriptConvert.SerializeObject(model);

            respose.AppendCookie(cookie);
        }
        /// <summary>
        /// 当前咨询客户的信息
        /// </summary>
        public static QuestionPersonModel getQuestionPersonModel
        {
            get
            {
                var Context = HttpContext.Current;
                if (Context.Request.Cookies != null && Context.Request.Cookies[COOKIES_KEY_QUESTION] != null)
                {
                    return JavaScriptConvert.DeserializeObject<QuestionPersonModel>(Context.Request.Cookies[COOKIES_KEY_QUESTION].Value);
                }
                return null;
            }
        }


        /// <summary>
        /// 退出咨询客户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool removeQuestion()
        {
            var result = false;
            try
            {
                var respose = HttpContext.Current.Response;
                respose.Cookies[COOKIES_KEY_QUESTION].Expires = DateTime.Now.AddDays(-1);
            }
            catch (Exception ex)
            {
                LogUtils.LogWriterToIISOut(ex);
            }
            return result;
        }

        public static void saveUserInfo(ServicePersonModel model)
        {
            var respose = HttpContext.Current.Response;
            // 存储登录状态
            respose.Cookies.Remove(COOKIES_KEY_ACCOUNT);
           

            HttpCookie cookie = new HttpCookie(COOKIES_KEY_ACCOUNT);

            cookie.Path = "/";
            //cookie.Expires = DateTime.Now.AddDays(saveDay);


            cookie.Value = JavaScriptConvert.SerializeObject(model);

            respose.AppendCookie(cookie);


            //缓存在线客服人员
            AutoServiceUtil.addOnline(model);


        }
        /// <summary>
        /// 当前登陆用户的信息
        /// </summary>
        public static ServicePersonModel getUserModel
        {
            get
            {
                var Context = HttpContext.Current;
                if (Context.Request.Cookies != null && Context.Request.Cookies[COOKIES_KEY_ACCOUNT] != null)
                {
                    return JavaScriptConvert.DeserializeObject<ServicePersonModel>(Context.Request.Cookies[COOKIES_KEY_ACCOUNT].Value); 
                }
                return null;
            }
        }
       

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool loginOut()
        {
            var result = false;
            try
            {
                
                AutoServiceUtil.delOnline(getUserModel);

                var respose = HttpContext.Current.Response;
                respose.Cookies[COOKIES_KEY_ACCOUNT].Expires = DateTime.Now.AddDays(-1);
                
                
            }
            catch (Exception ex)
            {
                LogUtils.LogWriterToIISOut(ex);
            }
            return result;
        }
       
        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool isLogin()
        {
            var result = false;
            try
            {
                var loginModel = getUserModel;
                if (loginModel != null && loginModel.loginname != "")
                {
                    AutoServiceUtil.addOnline(loginModel);
                    result = true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogWriterToIISOut(ex);
            }
            return result;
        }
    }
}