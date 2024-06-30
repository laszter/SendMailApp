using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

namespace SendMailApp
{
    public class SendMailNetMail
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

            string emails = config.sendTo;

            MailAddress FormAddress = new MailAddress(config.emailSender, config.senderDisplayName);
            MailMessage mail = new MailMessage();
            mail.From = FormAddress;
            mail.To.Add(emails);
            mail.Subject = config.emailTitle;
            mail.IsBodyHtml = true;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            if (filePathToAttach != "" && System.IO.File.Exists(filePathToAttach)) // thanakorn แนบไฟล์
            {
                Attachment attachment = new Attachment(filePathToAttach);
                mail.Attachments.Add(attachment);
            }

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(htmlView);
            SmtpClient smtp = new SmtpClient();

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = config.defaultCredentail;
            smtp.EnableSsl = config.enableSsl;
            smtp.Host = config.smtpHost;
            smtp.Port = config.smtpPort;
            if (config.defaultCredentail == false)
            {
                smtp.Credentials = new NetworkCredential(config.emailUsername, config.emailPassword);
            }
            smtp.Send(mail);
        }
    }
}