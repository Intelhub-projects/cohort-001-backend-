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


namespace Core.Appliaction.Implementation.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResponseService _sms;

        public ReminderService(IReminderRepository reminderRepository, IUserRepository userRepository, IResponseService sms)
        {
            _reminderRepository = reminderRepository;
            _userRepository = userRepository;
            _sms = sms;
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
                ReminderDays = request.ReminderDays
            };
            reminder.RemindDateAndTime = ConvertToString(request.RemindDateAndTime);
            var response = await _reminderRepository.AddAsync(reminder);
            if(reminder == null)
            {
                return new BaseResponse
                {
                    Message = "Creation Failed",
                    Status = false
                };
            }
            return new BaseResponse
            {
                Message = $"Succesfully Crete Reminder for {request.RemindFor}",
                Status = true
            };
        }

        private string ConvertToString(ICollection<DateTime> remindDateAndTime)
        {
            string a = "";
            foreach (var date in remindDateAndTime)
            {
                a += (Convert.ToString(date) + "  ");
            }
            return a;
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
                var time = reminder.RemindDateAndTime.Any(d => d.ToString("HH:mm") == DateTime.Now.ToString("HH:mm"));
                if (!time)
                {
                    continue;
                }
                var user = await _userRepository.GetUserAndRoles(reminder.userId);
                await _sms.SendResponse(user.PhoneNumber, reminder.RemindFor);
                if (reminder.RemindDateAndTime.Any(d => d.Date < DateTime.Now.Date))
                {
                    var remind = new Reminder
                    {
                        Id = reminder.Id,
                        UserId = reminder.userId,
                        ReminderStatus = ReminderStatus.Done,
                        ReminderDays = reminder.ReminderDays,
                        ReminderType = reminder.ReminderType,
                        RemindFor = reminder.RemindFor,
                    };
                    remind.RemindDateAndTime = ConvertToString(reminder.RemindDateAndTime);
                    await _reminderRepository.UpdateAsync(remind);
                }
            }
        }
        private async void WorkOnNonDailyReminder(IEnumerable<ReminderDto> dailyReminder)
        {
            foreach (var reminder in dailyReminder)
            {
                var time = reminder.RemindDateAndTime.Any(d => d.AddSeconds(-1 * d.Second) == DateTime.Now.AddSeconds(-1 * DateTime.Now.Second));
                if (!time)
                {
                    continue;
                }
                var user = await _userRepository.GetUserAndRoles(reminder.userId);
                await _sms.SendResponse(user.PhoneNumber, reminder.RemindFor);
                if (reminder.RemindDateAndTime.Any(d => d.Date < DateTime.Now.Date))
                {
                    var remind = new Reminder
                    {
                        Id = reminder.Id,
                        UserId = reminder.userId,
                        ReminderStatus = ReminderStatus.Done,
                        ReminderDays = reminder.ReminderDays,
                        ReminderType = reminder.ReminderType,
                        RemindFor = reminder.RemindFor,
                    };
                    remind.RemindDateAndTime = ConvertToString(reminder.RemindDateAndTime);
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
