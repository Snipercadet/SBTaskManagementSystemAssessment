using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GetDueDateNotificationTemplate(string name, string title, string description);
        string GetTaskCompletionNotificationTemplate(string name, string title, string description);
        string GetTaskAssignmentNotificationTemplate(string name, string title, string description,string dueDate);
    }
}
