using Application.DTOs;
using Application.Interfaces.Services;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SendMail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public string _mailKey;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailKey = _configuration.GetSection("MailConfiguration")["mailKey"];
        }

        public async Task<BaseResponse> SendWelcomeMailToNewPatient(string recipientName, string recipientMail, string text)
        {
            if (!Configuration.Default.ApiKey.ContainsKey("api-key"))
            {
                Configuration.Default.ApiKey.Add("api-key", _mailKey);
            }

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Intelhub Medpharm";
            string SenderEmail = "intelhubmedpharm@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(recipientMail, recipientName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string BccName = "Fmk";
            string BccEmail = "Folarinmuhammedfathi.k@gmail.com";
            SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
            List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();
            Bcc.Add(BccData);
            string CcName = "Abass";
            string CcEmail = "greatmoh007@gmail.com";
            SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            Cc.Add(CcData);
            string TextContent = null;
            string Subject = "My {{params.subject}}";
            string ReplyToName = "Intelhub Medpharm";
            string ReplyToEmail = "intelhubmedpharm@gmail.com";
            SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
            //string AttachmentUrl = "file:///C:/Users/Dejik%20Concepts/Documents/My%20CV.pdf";
            string AttachmentUrl = null;



            var stringInBase64 = Base64Encode(text);
            byte[] Content = System.Convert.FromBase64String(stringInBase64);
            string AttachmentName = "Welcome.txt";
            SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
            List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
            Attachment.Add(AttachmentContent);
            JObject Headers = new JObject();
            Headers.Add("Some-Custom-Name", "unique-id-1234");
            long? TemplateId = null;
            JObject Params = new JObject();
            Params.Add("parameter", "My param value");
            Params.Add("subject", "New Subject");
            List<string> Tags = new List<string>();
            Tags.Add("mytag");
            SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(recipientMail, recipientName);
            List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>();
            To1.Add(smtpEmailTo1);
            Dictionary<string, object> _parmas = new Dictionary<string, object>();
            _parmas.Add("params", Params);
            SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
            SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, Bcc, Cc, ReplyTo1, Subject);
            List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
            messageVersiopns.Add(messageVersion);
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, Bcc, Cc, "Caring for your life...", TextContent, Subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result.ToJson());
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }


            return new BaseResponse
            {
                Message = "Email sent successfully!",
                Status = true
            };

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }   }
