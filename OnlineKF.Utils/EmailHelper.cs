using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    /// <summary>
    /// 发送邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {

            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.qq.com"; //SMTP服务器
            string mailFrom = ConfigHelper.GetConfigString("CompanyEmail");   //登陆用户名
            string userPassword = ConfigHelper.GetConfigString("CompanyEmailPwd");   //登陆密码
            if (string.IsNullOrEmpty(mailFrom))
            {
                mailFrom = "investment@aeexgroup.com";
            }
            if (string.IsNullOrEmpty(userPassword))
            {
                userPassword = "Aeex17";
            }
            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }



        //判断字符串是否为邮箱格式
        public static bool IsEmail(string Str)
        {
            return Regex.IsMatch(Str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)" + @"|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }

}
