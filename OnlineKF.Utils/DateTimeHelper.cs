using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Globalization;
namespace OnlineKF.Utils
{
    public class DateTimeHelper
    {
        public DateTimeHelper() { }



        /// <summary>
        /// 将时间格式输出为字符串
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="strFormat">输出格式</param>
        /// <returns></returns>
        public static string GetTimeStrByFormat(DateTime dt, string strFormat)
        {
            return dt.ToString(strFormat, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 判断是否为日期格式
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        public static bool CheckDayFormat(string timeStr)
        {
            try
            {
                DateTime.Parse(timeStr);
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 取当天日期 格式：yyyy-MM-dd
        /// </summary>
        /// <returns>当天日期</returns>
        public static string GetCurrentDay()
        {
            DateTime dt = DateTime.Now;//默认为当天日期
            string currentDay = string.Format("{0:u}", dt).Substring(0, 10);

            return currentDay;
        }

        /// <summary>
        /// 以当天日期为坐标，取与当前日期相隔dayNum的日期 格式：yyyy-MM-dd
        /// </summary>
        /// <param name="dayNum">相隔日期 如：-1:昨天，1：第二天</param>
        /// <returns></returns>
        public static string GetDay(int dayNum)
        {
            DateTime dt = DateTime.Now;//默认为当天日期
            dt = dt.AddDays(dayNum);

            return string.Format("{0:u}", dt).Substring(0, 10);
        }


        /// <summary>
        /// 取两日期中间的所有日期集合 格式：yyyy-MM-dd 
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public static ArrayList GetDayList(string startDay, string endDay)
        {

            ArrayList list = new ArrayList();

            //转换为时间格式
            DateTime t1 = DateTime.Parse(startDay);
            DateTime t2 = DateTime.Parse(endDay);

            if (t1 > t2) t2 = t1;

            //循环
            for (DateTime i = t1; i <= t2; i = i.AddDays(1))
            {
                String lv_Day = i.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                list.Add(lv_Day);
            }
            return list;
        }

        /// <summary>
        /// 获得当前月，格式：yyyy-MM
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMonth()
        {

            DateTime dt = DateTime.Now;//默认为当天日期
            return string.Format("{0:u}", dt).Substring(0, 7);

        }

        /// <summary>
        /// 获取月份，格式：yyyy-MM
        /// </summary>
        /// <param name="monthNum">距当前月的月份 -1:上个月,1:下个月</param>
        /// <returns></returns>
        public static string GetMonth(int monthNum)
        {

            DateTime dt = DateTime.Now;
            dt = dt.AddMonths(monthNum);

            return string.Format("{0:u}", dt).Substring(0, 7);
        }

        /// <summary>
        /// 月份之间的列表(顺序排列)
        /// </summary>
        /// <param name="startMonth"></param>
        /// <param name="endMonth"></param>
        /// <returns></returns>
        public static ArrayList GetMonthList(string startMonth, string endMonth)
        {

            ArrayList list = new ArrayList();

            //转换为时间格式
            DateTime t1 = DateTime.Parse(startMonth);
            DateTime t2 = DateTime.Parse(endMonth);

            if (t1 > t2) t2 = t1;

            //循环
            for (DateTime i = t1; i <= t2; i = i.AddMonths(1))
            {
                string strMonth = i.ToString("yyyy-MM", DateTimeFormatInfo.InvariantInfo);
                list.Add(strMonth);
            }

            return list;
        }

        /// <summary>
        /// 月份之间的列表(倒序排列)
        /// </summary>
        /// <param name="startMonth"></param>
        /// <param name="endMonth"></param>
        /// <returns></returns>
        public static ArrayList GetMonthListDesc(string startMonth, string endMonth)
        {

            ArrayList list = new ArrayList();

            //转换为时间格式
            DateTime t1 = DateTime.Parse(startMonth);
            DateTime t2 = DateTime.Parse(endMonth);

            if (t1 > t2) t2 = t1;

            //循环
            for (DateTime i = t2; i >= t1; i = i.AddMonths(-1))
            {
                string strMonth = i.ToString("yyyy-MM", DateTimeFormatInfo.InvariantInfo);
                list.Add(strMonth);
            }

            return list;
        }
        /// <summary>
        /// 获取某个月的最后一天
        /// </summary>
        /// <param name="strMonth"></param>
        /// <returns></returns>
        public static string GetLastDayOfMonth(string strMonth)
        {

            DateTime d1 = DateTime.Parse(strMonth + "-01");//本月第1天
            DateTime d2 = d1.AddMonths(1).AddDays(-1);//本月最后一天

            string lastDay = d2.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            return lastDay;
        }

        /// <summary>
        /// 判断起始时间
        /// </summary>
        /// <param name="time1">开始日期yyyy-MM-dd</param>
        /// <param name="time2">结束日期yyyy-MM-dd</param>
        /// <returns>1:开始日期小于结束日期,0:开始日期等于结束日期,-1:开始日期大于结束日期</returns>
        public static int CheckDay(string time1, string time2)
        {

            DateTime stime = DateTime.Parse(time1);
            DateTime etime = DateTime.Parse(time2);

            return etime.CompareTo(stime);

        }

        /// <summary>
        /// 倒计时
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string GetEndTimeStr(DateTime dt)
        {
            //已到期
            if (dt.Ticks < DateTime.Now.Ticks)
            {
                return "已截止";
            }
            TimeSpan ts = dt - DateTime.Now;

            string str = "";
            int dayNum = int.Parse((ts.Days.ToString().PadLeft(2, '0')));
            if (dayNum == 0)
            {
                str = ts.Hours.ToString().PadLeft(2, '0') + "小时"
                    + ts.Minutes.ToString().PadLeft(2, '0') + "分"
                    + ts.Seconds.ToString().PadLeft(2, '0') + "秒";
            }
            else
            {
                str = dayNum.ToString() + "天"
                + ts.Hours.ToString().PadLeft(2, '0') + "小时"
                + ts.Minutes.ToString().PadLeft(2, '0') + "分"
                + ts.Seconds.ToString().PadLeft(2, '0') + "秒";
            }

            return str;

        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStick()
        {
            DateTime date = DateTime.Now;
            date = date.ToUniversalTime();
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = date - startDate;
            long times = (long)ts.TotalSeconds;
            return times;
        }

        /// <summary>
        /// 中文星期
        /// </summary>
        /// <returns></returns>
        public string GetDayOfWeekCN()
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[Convert.ToInt16(DateTime.Now.DayOfWeek)];
        }
        /// <summary>
        /// 获取星期数字形式,周一开始
        /// </summary>
        /// <returns></returns>
        public int GetDayOfWeekNum()
        {
            int day = (Convert.ToInt32(DateTime.Now.DayOfWeek) == 0) ? 7 : Convert.ToInt32(DateTime.Now.DayOfWeek);
            return day;
        }

    }
}
