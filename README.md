# SendMailApp
This is a .NET 5 Console App for testing sending email. 
# Initial Project
using:
```
dotnet restore
```
# Setting Config
File `config.json` you can edit config to send email
```
{
    "method": "",
    "message": "",
    "pathFile": "",
    "emailSender": "",
    "emailPassword": "",
    "senderDisplayName": "",
    "sendTo": "",
    "emailTitle": "",
    "smtpHost": "",
    "smtpPort": 587,
    "defaultCredentail": false,
    "enableSsl": true
}
```
- method : Which package sending email want to use. (NetMail, AIM)
- message : Text in body.
- pathFile : Path of file to attach.
- emailSender : email will send from.
- emailPassword : password of email sender.
- senderDisplayName : Display name of email sender.
- sendTo : email send to.
- emailTitle : Title of email.
- smtpHost : Server.
- smtpPort : port.
- defaultCredentail: setting for NetMail.
- enableSsl: setting for NetMail.