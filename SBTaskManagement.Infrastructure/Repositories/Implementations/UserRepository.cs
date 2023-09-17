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
    public class UserRepository :  Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<User> GetUserWithNavigationProp(Guid id)
        {
            var user = await dbset.Where(x => x.Id == id)
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersWithNavigationProp()
        {
            var users = await dbset.Where(x => x.Id != Guid.Empty)
                .Include(t => t.Tasks).ToListAsync();
            return users;
        }
    }
}
