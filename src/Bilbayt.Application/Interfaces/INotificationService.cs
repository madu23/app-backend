using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bilbayt.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendEmailAsync(string fromEmail, string toEmail, string subject, string message);
    }
}
