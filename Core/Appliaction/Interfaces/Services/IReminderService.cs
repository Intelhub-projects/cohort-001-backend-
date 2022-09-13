using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Filters;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Appliaction.DTOs.RequestModel;
using Core.Appliaction.DTOs.ResponseModel;
using Core.Domain.Enums;

namespace Core.Appliaction.Interfaces.Services
{
    public interface IReminderService
    {
        Task<BaseResponse> CreateAsync(Guid userId, CreateReminder request);
        Task<PaginatedList<ReminderDto>> GetOnboardReminderByUserIdAsync(Guid userId, PaginationFilter filter);
        Task<PaginatedList<ReminderDto>> GetDoneReminderByUserIdAsync(Guid userId, PaginationFilter filter);
        Task<IEnumerable<ReminderDto>> GetAllReminderAsync();
        Task<IList<Result<ReminderDto>>> GetAllRemindersByStatusAsync(ReminderStatus status);
    }
}
