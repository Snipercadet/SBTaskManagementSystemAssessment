using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SBTaskManagement.Application.Services.Interfaces;

namespace SBTaskManagement.Infrastructure.DBContext
{
    public class AppDbContext : DbContext, IPersistenceAudit
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IPersistenceAudit persistenceAudit) : base(options)
        {
            GetCreatedById = persistenceAudit.GetCreatedById;
        }
       

        public Guid? GetCreatedById { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Task>()
               .HasOne(t => t.User)
               .WithMany(u=>u.Tasks)
               .HasForeignKey(t => t.UserId);


            modelBuilder.Entity<Domain.Entities.Task>()
                .HasOne(t => t.Project)
                .WithMany(u => u.TasksList)
                .HasForeignKey(t => t.ProjectId);
                
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Auditable details entity pre-processing
            Audit();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity
                                                             && (x.State == EntityState.Modified
                                                             || x.State == EntityState.Added));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IAuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((IAuditableEntity)entry.Entity).CreatedById = GetCreatedById;
                }
                ((IAuditableEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
