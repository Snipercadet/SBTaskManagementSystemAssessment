using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMail(string reciepientAddress, string message,
            string subject);
    }
}
