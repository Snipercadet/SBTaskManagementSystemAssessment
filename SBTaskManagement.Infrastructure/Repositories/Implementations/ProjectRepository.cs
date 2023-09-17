using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Infrastructure.DBContext;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Implementations
{
    public class ProjectRepository :  Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<Project> GetProductWithListOfTasks(Guid id)
        {
            var listOfProjectTask = await dbset.Where(x => x.Id == id)
                .Include(t => t.TasksList)
                .FirstOrDefaultAsync();
            return listOfProjectTask;
        }
    }
}
