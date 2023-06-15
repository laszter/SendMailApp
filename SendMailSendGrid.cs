using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendMailApp{
    public class SendMailSendGrid{
        public static async Task SendAsync(string message, string filePathToAttach){
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
            string senderUserNameBMS = config.senderDisplayName;
            string emails = config.sendTo;
            
            var client = new SendGridClient(config.sendGridApiKey);
            var msg = new SendGridMessage();
            msg.From = new EmailAddress(sendFrom, senderUserNameBMS);
            msg.Subject = config.emailTitle;
            msg.HtmlContent = htmlBody;

            if (filePathToAttach != "" && System.IO.File.Exists(filePathToAttach))
            {
                SendGrid.Helpers.Mail.Attachment attachment = new SendGrid.Helpers.Mail.Attachment();
                var bytes = File.ReadAllBytes(filePathToAttach);
                var file = Convert.ToBase64String(bytes);
                attachment.Content = file;
                attachment.Filename = Path.GetFileName(filePathToAttach);

                msg.AddAttachment(attachment);
            }

            msg.AddTo(new EmailAddress(emails));
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Headers);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result);
        }
    } 
}