using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Appliaction.Interfaces.Services;
using Infrastructure.BackgroundServices.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;

namespace Infrastructure.BackgroundServices
{
    public class RemindersTasks : BackgroundService
    {
        private readonly MedPharmCronExpressionConfig _configuration;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private readonly ILogger<RemindersTasks> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RemindersTasks(IOptions<MedPharmCronExpressionConfig> configuration, 
            ILogger<RemindersTasks> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration.Value;
            _schedule = CrontabSchedule.Parse(_configuration.CronExpression);
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
                        reminderService.SendAlert();
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured generating the weekly payroll. {ex.Message}");
                    _logger.LogError(ex, ex.Message);
                }
                _logger.LogInformation($"Background hosted service for {nameof(RemindersTasks)} is stopping");
                var timespan = _nextRun - now;
                await Task.Delay(timespan, stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);


            }
        }
    }
}
