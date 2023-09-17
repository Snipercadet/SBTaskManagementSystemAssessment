using SBTaskManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.DTOs
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public EPriority Priority { get; set; }
        public ETaskStatus Status { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ICollection<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();

    }

    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        internal DateTime DueDate { get; set; }
        public EPriority Priority { get; set; }
        public ETaskStatus Status { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
    }

    public class UpdateTaskDto : CreateTaskDto
    {
    }


    public class SimpleStatusOrPriority
    {
        private ETaskStatus _taskStatus;
        public string TaskStatus { get; set; }
        private EPriority _taskPriority;
        public string Priority { get; set; }
    }

}
