using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Domain.Entities
{
    public  class Task : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(3).ToUniversalTime();
        public EPriority Priority { get; set; } = EPriority.Low;
        public ETaskStatus Status { get; set; } = ETaskStatus.Pending;

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }
        public bool isNotificationSent { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
