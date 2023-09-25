using System;
using System.Globalization;
using System.IO;
using AegisImplicitMail;
using Newtonsoft.Json;

namespace SendMailApp
{
    public class SendMailAIM
    {
        public static void Send(string message, string filePathToAttach)
        {
            string dateString = DateTime.Now.ToString("วันที่ d MMMM yyyy เวลา HH:mm น.", CultureInfo.CreateSpecificCulture("th-TH"));

            string htmlBody = $@"
                ส่งเมลมา ณ เวลา {dateString}
                <br />
                <br />
                <div style=""padding: 20px; margin: 10px; background-color:rgb(240,240,240);"">
                {message}
                </div>
                ";

            string textConfig = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(textConfig);

            string sendFrom = config.emailSender;
            string Username = config.emailUsername;
            string Password = config.emailPassword;
            string senderUserNameBMS = config.senderDisplayName;
            string emails = config.sendTo;

            MimeMailAddress FormAddress = new MimeMailAddress(sendFrom, senderUserNameBMS);
            MimeMailMessage mail = new MimeMailMessage();
            mail.From = FormAddress;
            mail.To.Add(emails);
            mail.Subject = config.emailTitle;
            mail.IsBodyHtml = true;
            mail.Body = htmlBody;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            if (filePathToAttach != "" && System.IO.File.Exists(filePathToAttach)) // thanakorn แนบไฟล์
            {
                MimeAttachment attachment = new MimeAttachment(filePathToAttach);
                mail.Attachments.Add(attachment);
            }

            string host = config.smtpHost;
            int port = config.smtpPort;
            MimeMailer mailer = new MimeMailer(host, port);

            mailer.User = Username;
            mailer.Password = Password;
            mailer.SslType = port == 465 ? SslMode.Ssl : SslMode.Tls;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            mailer.SendMail(mail);
        }
    }
}