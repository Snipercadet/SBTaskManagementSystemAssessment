using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SBTaskManagement.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public Guid Id { get; set; }
        public ENotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool IsNotificationSent { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Guid? UserId { get; set; }
        public User User { get; set; }
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public string Errors { get; set; }

    }
}
