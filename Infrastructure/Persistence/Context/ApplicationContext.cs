using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Core.Domain.Entities;

namespace Persistence.Context
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyAllConfigurations<ApplicationContext>();
            builder.ConfigureDeletableEntities();
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"

                },
                  new Role
                  {
                      Id = Guid.NewGuid(),
                      Name = "Patient"
                  },
                   new Role
                   {
                       Id = Guid.NewGuid(),
                       Name = "Staff"
                   }
                );
            builder.Entity<Message>().HasData(
                new Message
                {
                    Id = Guid.NewGuid(),
                    MessageType = Core.Domain.Enums.MessageType.RegistrationMessage,
                    Text = "Welcome to MedPharm. On behalf of all our staffs, I hope you will be having a wonderful health care here. " +
                    "Management and staff are pleased to be your at your services.I look forward to demonstrating services and skills distinctive and special to MedPharm" +
                    "which is fully equipped with Health club facilities to cater to our users."
                    
                }


                );

                builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            this.AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            this.AddAuditInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess,
                cancellationToken);
        }

        private const string IsDeletedProperty = "IsDeleted";
        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[IsDeletedProperty] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[IsDeletedProperty] = true;
                        break;
                }
            }
        }
        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set;}
        public DbSet<UserRole> UserRoles {get; set;}
        public DbSet<Reminder> Reminders {get; set;}
        public DbSet<Message> Messages { get; set; }
    }
}
