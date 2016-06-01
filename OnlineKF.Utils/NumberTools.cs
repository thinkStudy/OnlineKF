using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    /// <summary>
    /// 数字相关工具类
    /// </summary>
    public class NumberTools
    {
        public NumberTools()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 将对象转换为整形，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int GetInt(Object src)
        {
            try
            {
                return Convert.ToInt32(src);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转换为整形，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static short GetShort(Object src)
        {
            try
            {
                return Convert.ToInt16(src);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 对象转换为长整形，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static long GetLong(Object src)
        {
            try
            {
                return Convert.ToInt64(src);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        /// <summary>
        /// 对象转换为double型，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static double GetDouble(Object src)
        {
            try
            {
                return Convert.ToDouble(src);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 对象转换为decimal型，转换失败则返回0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static decimal GetDecimal(Object src) {

            try
            {
                return Convert.ToDecimal(src);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }





        /// <summary>
        /// 求百分率
        /// </summary>
        /// <param name="a">分子</param>
        /// <param name="b">分母</param>
        /// <returns>58.23%</returns>
        public static string GetPercentage(int a, int b)
        {

            if (b == 0) return "0.00%";
            decimal percentageRate = decimal.Round(decimal.Parse((100 * a).ToString()) / b, 2);
            return percentageRate.ToString() + "%";
        }


        /// <summary>
        /// 转化格式
        /// </summary>
        /// <returns></returns>
        public static string GetStringNum(object inNum)
        {
            try
            {
                return string.Format("{0:n0}", inNum);
            }
            catch //(Exception ex)
            {
                return inNum.ToString();
            }
        }

        /// <summary>
        /// 转化数字格式
        /// </summary>
        /// <param name="inNum">输入</param>
        /// <param name="format">格式</param>
        /// <returns>按格式输出</returns>
        public static string GetStringNum(object inNum,string format)
        {
            try
            {
                //format ＝ "{0:n0}"; -- 不保留小数，用分号隔开的数字
                //format ＝ "{0:n}"; -- 保留小数，用分号隔开的数字
                //format ＝ "{0:C3}"; -- 货币
                return string.Format(format, inNum);
            }
            catch //(Exception ex)
            {
                return inNum.ToString();
            }
        }

        /// <summary>
        /// 用单位分转为单位元（除以100）
        /// </summary>
        /// <param name="inNum"></param>
        /// <returns></returns>
        public static decimal GetRmbNum(object inNum)
        {
            try
            {

                decimal outNum = decimal.Round(decimal.Parse(inNum.ToString()) / 100, 2);
                return outNum;

                //return string.Format("{0:N0}", outNum);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
