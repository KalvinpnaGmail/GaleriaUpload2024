﻿using MailKit.Net.Smtp;
using MimeKit;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Helpers
{
    public class MailHelper : ImailHelpers
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body)
        {
            try
            {
                var from = _configuration["Mail:From"];
                var name = _configuration["Mail:Name"];
                var smtp = _configuration["Mail:Smtp"];
                var port = _configuration["Mail:Port"];
                var password = _configuration["Mail:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, from));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port!), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new ActionResponse<string> { WasSuccess = true };
            }
            catch (Exception ex)
            {
                return new ActionResponse<string>
                {
                    WasSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}