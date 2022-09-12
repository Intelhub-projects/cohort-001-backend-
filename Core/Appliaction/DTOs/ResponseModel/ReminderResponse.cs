using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Core.Appliaction.DTOs.ResponseModel
{
    public class ReminderResponse : BaseResponse
    {
        public ICollection<ReminderDto> reminderDtos { get; set; }
    }
}
