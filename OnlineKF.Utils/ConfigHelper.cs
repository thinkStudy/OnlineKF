using System;
using System.Configuration;

namespace OnlineKF.Utils
{
	/// <summary>
	/// web.config操作类
    /// Copyright (C) Maticsoft
	/// </summary>
	public sealed class ConfigHelper
	{
		/// <summary>
		/// 得到AppSettings中的配置字符串信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetConfigString(string key)
		{
            string CacheKey = "AppSettings-" + key;
            object objModel = DataCache.GetCache(CacheKey);
            string result = "";
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        result = objModel.ToString();
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }

                }
                catch
                { }
            }
            else {
                result = objModel.ToString();
            }
            return result;
		}

		/// <summary>
		/// 得到AppSettings中的配置Bool信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetConfigBool(string key)
		{
			bool result = false;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = bool.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}
			return result;
		}
		/// <summary>
		/// 得到AppSettings中的配置Decimal信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static decimal GetConfigDecimal(string key)
		{
			decimal result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = decimal.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}

			return result;
		}
		/// <summary>
		/// 得到AppSettings中的配置int信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int GetConfigInt(string key)
		{
			int result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = int.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}

			return result;
		}
        /// <summary>
        /// 获取日志存储路径
        /// </summary>
        /// <returns></returns>
        public static string GetLogPath()
        {
            string result =  GetConfigString("LogPath");
            if (string.IsNullOrEmpty(result)) { 
                result = AppDomain.CurrentDomain.BaseDirectory+"error_log";
            }
            return result;

        }

        /// <summary>
        /// 获取资源的路径
        /// </summary>
        /// <returns></returns>
        public static string GetResourcePath()
        {
            return GetConfigString("ResourcePath");
            
        }

        /// <summary>
        /// 获取资源的Web站点
        /// </summary>
        /// <returns></returns>
        public static string GetResourceWebPath()
        {
            return GetConfigString("ResourceWeb");
        }


        /// <summary>
        /// 获取项目展示的Web站点
        /// </summary>
        /// <returns></returns>
        public static string GetShowCompanyWeb()
        {
            return GetConfigString("ShowCompanyWeb");
        }
	}
}
