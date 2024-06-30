using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace SendMailApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("config.json"))
            {
                Console.WriteLine("Not Found Config");
                Console.WriteLine("This console will be terminated in 5 seconds.");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            string textConfig = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(textConfig);

            try
            {
                switch (config.method.ToLower())
                {
                    case "aim":
                        SendMailAIM.Send(config.message, config.pathFile);
                        break;
                    case "netmail":
                        SendMailNetMail.Send(config.message, config.pathFile);
                        break;
                    case "sendgrid":
                        SendMailSendGrid.SendAsync(config.message, config.pathFile).Wait();
                        break;
                }
                Console.WriteLine("No Error Found");
                // Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Console.ReadKey();
            }
        }
    }

    class Config
    {
        public string emailSender { get; set; }
        public string emailUsername { get; set; }
        public string emailPassword { get; set; }
        public string senderDisplayName { get; set; }
        public string sendTo { get; set; }
        public string emailTitle { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public string message { get; set; }
        public string pathFile { get; set; }
        public string method { get; set; }
        public bool defaultCredentail { get; set; }
        public bool enableSsl { get; set; }
        public string sendGridApiKey { get; set; }
    }
}
