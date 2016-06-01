using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Net.NetworkInformation;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取本机IP地址信息
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string IPstr = "";
            string hostName = Dns.GetHostName();//本机名   
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            IPstr = addressList[2].ToString();
            return IPstr;
        }
     
    }
}
