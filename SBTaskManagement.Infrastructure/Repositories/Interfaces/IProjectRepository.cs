using SBTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Interfaces
{
    public interface IProjectRepository:IRepository<Project>
    {
        Task<Project> GetProductWithListOfTasks(Guid id);
    }
}
