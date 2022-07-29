using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Runtime.InteropServices;

namespace SchoolTakeout.Tool
{
    public class SendEmail
    {
        public  bool sendEmail(string content)
        {
            var emailAcount = ConfigurationManager.AppSettings["EmailAcount"];
            var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            //var reciver = "1963339038@qq.com";
            var reciver1 = "2406812549@qq.com";
            var reciver2 = "2728663470@qq.com";
            var reciver3 = "1432494165@qq.com";
            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress("1432494165@qq.com");
            message.From = fromAddr;
            //设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(reciver1);
            message.To.Add(reciver2);
            message.To.Add(reciver3);
            //设置抄送人
            // message.CC.Add("izhaofu@163.com");
            //设置邮件标题

            message.Subject = "订单提醒-"+DateTime.Now.ToString();
            //设置邮件内容
            message.Body = content;
            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            SmtpClient client = new SmtpClient("smtp.qq.com ", 587);
            //设置发送人的邮箱账号和密码
            client.Credentials = new NetworkCredential("1432494165@qq.com", "odfkiwbiujyojebf");
            //启用ssl,也就是安全发送
            client.EnableSsl = true;
            //发送邮件\
            try
            {
                client.Send(message);
                // Console.WriteLine("success");
                return true;
            }
            catch (Exception)
            {
                //Console.WriteLine("faile");
                //Console.WriteLine(e);
                //Console.ReadLine();
                return false;
            }

        }
    }
}