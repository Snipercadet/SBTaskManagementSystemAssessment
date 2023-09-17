using SBTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITaskRepository:IRepository<Domain.Entities.Task>
    {
        Task<Domain.Entities.Task> GetTaskWithNavigationProp(Guid id);
        Task<List<Notification>> GetTaskNotificationForNewAssignment();
        Task<List<Domain.Entities.Task>> GetTaskNotificationCompleted();
    }
}
