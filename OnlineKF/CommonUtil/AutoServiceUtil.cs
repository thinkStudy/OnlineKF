using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineKF.Utils;
using OnlineKF.Model;
using System.Web.Caching;

namespace OnlineKF.CommonUtil
{
    /// <summary>
    /// 自动分配客服中心
    /// </summary>
    public static class AutoServiceUtil
    {
        public static string SERVICEONLINEINFO_KEY = "SERVICE_ONLINE_INFO";

        /// <summary>
        /// 添加在线客服
        /// </summary>
        /// <param name="model"></param>
        public static void addOnline( ServicePersonModel model)
        {
            IList<ServicePersonModel> serviceList = allPerson;
            if (serviceList == null)
            {
                serviceList = new List<ServicePersonModel>();
                serviceList.Add(model);
            }
            else {
                var hasAdd = serviceList.Where(s => s.loginname == model.loginname).ToList<ServicePersonModel>();
                if (hasAdd == null || hasAdd.Count < 1) {
                    serviceList.Add(model);
                }
            }
            allPerson = serviceList;
        }
        /// <summary>
        /// 删除在线客服
        /// </summary>
        /// <param name="model"></param>
        public static void delOnline(ServicePersonModel model)
        {
            var serviceList = allPerson;
            if (serviceList != null)
            {
                for (var i = 0; i < serviceList.Count; i++) {
                    if (serviceList[i].id == model.id) {
                        serviceList.RemoveAt(i);
                        break;
                    }
                }
                allPerson = serviceList;
            }
        }
        /// <summary>
        /// 当前登陆客服的信息
        /// </summary>
        public static IList<ServicePersonModel> allPerson
        {
            get
            {
                if (DataCache.GetCache(SERVICEONLINEINFO_KEY) != null)
                {
                   return DataCache.GetCache(SERVICEONLINEINFO_KEY) as IList<ServicePersonModel>;
                }
                return null;
            }
            set {
               
                HttpRuntime.Cache.Add(SERVICEONLINEINFO_KEY, value,
                       null,
                       DateTime.Now.AddHours(24),
                       Cache.NoSlidingExpiration,
                       CacheItemPriority.High,
                       null);
               
            }
        }

        /// <summary>
        /// 分配在线客服人员ID
        /// </summary>
        /// <returns>客服人员ID</returns>
        public static int getOnlinePerson(int companyid) {

            var result = 0;

            var Session = HttpContext.Current.Session;
            IList<ServicePersonModel> entityList = allPerson;
            try
            {
                //分配当前在线客服
                if (entityList != null && entityList.Count > 0)
                {
                    var newList = entityList.Where(l=>l.compayid == companyid).ToList<ServicePersonModel>();
                    
                    if(newList != null && newList.Count > 0 ){
                        newList.OrderByDescending(s => s.serviceNumber).ToList<ServicePersonModel>();
                        result = newList[0].id;
                        newList[0].serviceNumber = newList[0].serviceNumber + 1;
                        allPerson = newList;
                    }
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