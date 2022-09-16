using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Application.Wrapper;
using Core.Appliaction.Constants;
using Core.Appliaction.DTOs;
using Core.Appliaction.DTOs.RequestModel;
using Core.Appliaction.Interfaces.Repository;
using Core.Appliaction.Interfaces.Services;

using Core.Domain.Enums;

using Core.Domain.Entities;
using Task = Core.Domain.Entities.MyTask;

namespace Core.Appliaction.Implementation.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResponseService _sms;
        private readonly ITaskRepository _task;

        public ReminderService(IReminderRepository reminderRepository, IUserRepository userRepository, IResponseService sms, ITaskRepository task)
        {
            _reminderRepository = reminderRepository;
            _userRepository = userRepository;
            _sms = sms;
            _task = task;
        }

        public async Task<BaseResponse> CreateAsync(Guid userId, CreateReminder request)
        {
            var foundUser = await _userRepository.AnyAsync(u => u.Id == userId);
            if (foundUser == false || request == null)
            {
                return new BaseResponse
                {
                    Message = "Request Fail, Check User Id and request",
                    Status = false
                };
            }
            var user = await _userRepository.GetUserAndRoles(userId);

            var reminder = new Reminder
            {
                UserId = userId,
                RemindFor = request.RemindFor,
                ReminderStatus = Domain.Enums.ReminderStatus.Onboard,
                ReminderType = request.ReminderType,
                ReminderDays = request.ReminderDays,
            };
            var response = await _reminderRepository.AddAsync(reminder);
            if (response.Id == Guid.Empty)
            {
                return new BaseResponse
                {
                    Message = "Creation Failed",
                    Status = false
                };
            }
            foreach (var t in request.Tasks)
            {
                var tas = new MyTask
                {
                    Todo = t.Todo,
                    TodoTime = t.TodoTime,
                    ReminderId = reminder.Id
                };
                await _task.AddAsync(tas);
            }

            
            return new BaseResponse
            {
                Message = $"Succesfully Crete Reminder for {request.RemindFor}",
                Status = true
            };
        }

  
        public Task<IEnumerable<ReminderDto>> GetAllReminderAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReminderDto>> GetAllRemindersByStatusAsync(ReminderStatus status)
        {
            return await _reminderRepository.GetAllRemindersByStatusAsync(status);
   
        }

        public async void SendAlert()
        {
            var reminders = await _reminderRepository.GetAllRemindersByStatusAsync(ReminderStatus.Onboard);
            var nonDaily = reminders.Where(t => t.ReminderType == ReminderType.NonDaily);
            var daily = reminders.Where(t => t.ReminderType == ReminderType.Daily);

            WorkOnDailyReminder(daily);
            WorkOnNonDailyReminder(nonDaily);
           
        }

        private async void WorkOnDailyReminder(IEnumerable<ReminderDto> dailyReminder)
        {
            foreach (var reminder in dailyReminder)
            {
                var time = reminder.Tasks.Any(d => d.TodoTime.ToString("HH:mm") == DateTime.Now.ToString("HH:mm"));
                if (!time)
                {
                    continue;
                }
                var user = await _userRepository.GetUserAndRoles(reminder.userId);
                await _sms.SendResponse(user.PhoneNumber, reminder.RemindFor);               
                
            }
        }
        private async void WorkOnNonDailyReminder(IEnumerable<ReminderDto> dailyReminder)
        {
            foreach (var reminder in dailyReminder)
            {
                var time = reminder.Tasks.Any(d => d.TodoTime.AddSeconds(-1 * d.TodoTime.Second) == DateTime.Now.AddSeconds(-1 * DateTime.Now.Second));
                if (!time)
                {
                    continue;
                }
                var user = await _userRepository.GetUserAndRoles(reminder.userId);
                await _sms.SendResponse(user.PhoneNumber, reminder.RemindFor);
                if (reminder.Tasks.Count(d => d.TodoTime.Date < DateTime.Now.Date) == reminder.Tasks.Count())
                {
                    var remind = new Reminder
                    {
                        Id = reminder.Id,
                        ReminderStatus = ReminderStatus.Done,
                    };
                    await _reminderRepository.UpdateAsync(remind);
                }
            }
        }
        
        public async Task<PaginatedList<ReminderDto>> GetOnboardReminderByUserIdAsync(Guid userId, PaginationFilter filter)
        {
            var doneReminders = await _reminderRepository.GetAllUserReminderByStatusAsync(u => u.Id == userId && u.ReminderStatus == ReminderStatus.Onboard, filter);
            return doneReminders;
        }

        public async Task<PaginatedList<ReminderDto>> GetDoneReminderByUserIdAsync(Guid userId, PaginationFilter filter)
        {
            var doneReminders = await _reminderRepository.GetAllUserReminderByStatusAsync(u => u.Id == userId && u.ReminderStatus == ReminderStatus.Done, filter);
            return doneReminders;
        }
    }
}
