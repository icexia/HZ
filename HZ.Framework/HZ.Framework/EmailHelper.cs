using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework
{
    public class EmailHelper
    {
        public static void SendEmail(string mailBody)
        {
            //读取是否需要预警邮件的配置
            string strIsEnable = ConfigurationManager.AppSettings["BusAlarmEnable"];
            if (strIsEnable != null && strIsEnable == "false")
                return;

            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress("lingjiexia@126.com"); //发送人邮箱地址

            string receiveUsers = ConfigurationManager.AppSettings["BusAlarmReceiveUsers"];

            string[] users = receiveUsers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string user in users)
            {
                mailObj.To.Add(user);
            }
            ////收件人邮箱地址
            //mailObj.To.Add("xxx@126.com");


            mailObj.Subject = "邮件助手";    //主题
            mailObj.Body = mailBody;  //正文
            mailObj.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.exmail.qq.com";         //smtp服务器名称
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("lingjiexia@126.com", "123456");  //发送人的登录名和密码
            smtp.Send(mailObj);
        }

        public static void SendEmail(string subject, string mailBody, List<string> receiveUsers)
        {
            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress("lingjiexia@126.com"); //发送人邮箱地址
            if (receiveUsers != null && receiveUsers.Count > 0)
            {
                foreach (string user in receiveUsers)
                {
                    mailObj.To.Add(user);
                }

                mailObj.Subject = subject;    //主题
                mailObj.Body = mailBody;  //正文
                mailObj.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.exmail.qq.com";         //smtp服务器名称
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("lingjiexia@126.com", "123456");  //发送人的登录名和密码
                smtp.Send(mailObj);
            }
        }
    }
}
