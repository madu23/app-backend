using Bilbayt.Application.Interfaces;
using Bilbayt.Application.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bilbayt.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly SendGridSetting _sendGridSetting;
        public NotificationService(IOptions<SendGridSetting> options)
        {
            _sendGridSetting = options.Value;
        }
        public async Task<bool> SendEmailAsync(string fromEmail, string toEmail, string subject, string message)
        {
            var apiKey = _sendGridSetting.Key;
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, "Oriaku Eke");
                var sub = subject;
                var to = new EmailAddress(toEmail, "Example User");
                var plainTextContent = message;
                var htmlContent = string.Empty;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // TODO: You can log error to elastic search or file using serilog for .net core. For simplicity sake let throw out the error and let user know something went wrong.

                throw ex;
            }

        }
    }
}