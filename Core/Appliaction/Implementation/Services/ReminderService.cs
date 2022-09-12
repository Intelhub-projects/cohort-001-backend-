﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Appliaction.DTOs.RequestModel;
using Core.Appliaction.Interfaces.Repository;
using Core.Appliaction.Interfaces.Services;

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

        public Task<BaseResponse> CreateAsync(Guid userId, CreateReminder request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReminderDto>> GetAllReminderAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<ReminderDto>> GetReminderByUserIdAsync(Guid userId, PaginationFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}