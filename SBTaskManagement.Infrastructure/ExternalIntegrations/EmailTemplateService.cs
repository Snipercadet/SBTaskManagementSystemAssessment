using Microsoft.Extensions.Configuration;
using SBTaskManagement.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.ExternalIntegrations
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetDueDateNotificationTemplate(string name, string title, string description)
        {
            string body;
            var folderName = Path.Combine("wwwroot", "DueDateNotification.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return null;

            string msgBody = body.Replace("{full_name}", name)
                .Replace("{title}", title)
                .Replace("{description}", description);

            return msgBody;
        }

        public string GetTaskCompletionNotificationTemplate(string name, string title, string description)
        {
            string body;
            var folderName = Path.Combine("wwwroot", "TaskCompletionNotification.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return null;

            string msgBody = body.Replace("{full_name}", name)
                .Replace("{title}", title)
                .Replace("{description}", description);

            return msgBody;
        }

        public string GetTaskAssignmentNotificationTemplate(string name, string title, string description, string dueDate)
        {
            string body;
            var folderName = Path.Combine("wwwroot", "DueDateNotification.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return null;

            string msgBody = body.Replace("{full_name}", name)
                .Replace("{title}", title)
                .Replace("{description}", description)
                .Replace("{due_date}", dueDate);

            return msgBody;
        }
    }
}
