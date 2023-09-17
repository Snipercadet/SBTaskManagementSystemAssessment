using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Domain.Enums;
using SBTaskManagement.Infrastructure.DBContext;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Implementations
{
    public class TaskRepository :  Repository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<Domain.Entities.Task> GetTaskWithNavigationProp(Guid id)
        {
            var listOfProjectTask = await dbset.Where(x => x.Id == id)
                .Include(t => t.Project)
                .Include(u=>u.User)
                .FirstOrDefaultAsync();
            return listOfProjectTask;
        }

        public async Task<List<Notification>> GetTaskNotificationForNewAssignment()
        {
            var listOfProjectTask = await dbset.Where(x => x.Id != Guid.Empty)
                .SelectMany(t => t.Notifications)
                .Where(t => t.Type == Domain.Enums.ENotificationType.NewTaskAssignment && t.IsNotificationSent == false)
                .Include(c => c.User)
                .ToListAsync();

            return listOfProjectTask;
            
        }

        public async Task<List<Domain.Entities.Task>> GetTaskNotificationCompleted()
        {
            var listOfProjectTask = await dbset.Where(x => x.Id != Guid.Empty && x.isNotificationSent == false)
                .Take(20)
                .Where(t => t.Status == ETaskStatus.Completed)
                .Include(c => c.User)
           
                .ToListAsync();

            return listOfProjectTask;

        }
    }
}
