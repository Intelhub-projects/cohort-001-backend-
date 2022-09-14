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

        public ReminderService(IReminderRepository reminderRepository, IUserRepository userRepository)
        {
            _reminderRepository = reminderRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> CreateAsync(Guid userId, CreateReminder request)
        {
            if (!await _userRepository.AnyAsync(u => u.Id == userId) || request == null)
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
