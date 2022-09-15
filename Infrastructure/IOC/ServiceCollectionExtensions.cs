using System.ComponentModel.Design;
using Application.Implementations.Services;
using Application.Interfaces.Identity;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Appliaction.Implementation.Services;
using Core.Appliaction.Interfaces.Repository;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Infrastructure.BackgroundServices;
using Infrastructure.BackgroundServices.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Identity;
using Persistence.Repositories;
using Infrastructure.SendMail;

namespace IOC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReminderService, ReminderService>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMailService, MailService>();
            return services;
        }
        
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();
            services.AddScoped<IIdentityService, IdentityService>();

            services.Configure<MedPharmCronExpressionConfig>(Configuration.GetSection("SmsBackgroundConfiguration"));
            services.AddHostedService<RemindersTasks>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}