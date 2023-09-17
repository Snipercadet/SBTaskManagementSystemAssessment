using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Infrastructure.DBContext;
using SBTaskManagement.Infrastructure.ExternalIntegrations;
using SBTaskManagement.Infrastructure.Repositories.Implementations;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using SBTaskManagement.Infrastructure.ServiceImplementation;
using SBTaskManagement.Infrastructure.ServicesImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure
{
    public static class ServiceCollection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService,TaskService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPersistenceAudit, PersistenceAuditService>();
            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
          
            services.AddLogging();
           
            return services;
        }

        public static IServiceCollection AddClientDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>((option) =>
            {
                option.UseNpgsql(connectionString);
            });
            return services;
        }
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        public static IServiceCollection RegisterExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            return services;
        }
    }
}
