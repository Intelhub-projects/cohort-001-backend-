﻿using System;
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

namespace Core.Appliaction.Interfaces.Services
{
    public interface IReminderService
    {
        Task<BaseResponse> CreateAsync(CreateReminder request);
        Task<PaginatedList<ReminderDto>> GetReminderByUserIdAsync(Guid userId, PaginationFilter filter);
        Task<IEnumerable<UserDto>> GetAllReminderAsync();
    }
}
