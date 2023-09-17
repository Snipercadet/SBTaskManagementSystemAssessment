using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public ENotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool IsNotificationSent { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
    }

    public class CreateNotificationDto
    {
        public ENotificationType Type { get; set; }
        public string Message { get; set; }
        public Guid? UserId { get; set; }
        public Guid? TaskId { get; set; }
    }

    public class UpdateNotificationDto : CreateNotificationDto { }
}
