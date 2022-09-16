using Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EnumController : ControllerBase
    {
        [HttpGet("GetGenders")]
        public IActionResult GetGenders()
        {
            var genders = Enum.GetValues(typeof(Gender)).Cast<int>().ToList();
            List<string> gender = new List<string>();
            foreach (var item in genders)
            {
                gender.Add(Enum.GetName(typeof(Gender), item));
            }
            return Ok(gender);

        }

        [HttpGet("GetMessageType")]
        public IActionResult GetMessageType()
        {
            var messages = Enum.GetValues(typeof(MessageType)).Cast<int>().ToList();
            List<string> message = new List<string>();
            foreach (var item in messages)
            {
                message.Add(Enum.GetName(typeof(MessageType), item));
            }
            return Ok(message);

        }

        [HttpGet("GetReminderType")]
        public IActionResult GetReminderType()
        {
            var reminders = Enum.GetValues(typeof(ReminderType)).Cast<int>().ToList();
            List<string> reminder = new List<string>();
            foreach (var item in reminders)
            {
                reminder.Add(Enum.GetName(typeof(ReminderType), item));
            }
            return Ok(reminder);

        }

        [HttpGet("GetReminderStatus")]
        public IActionResult GetReminderStatus()
        {
            var reminders = Enum.GetValues(typeof(ReminderStatus)).Cast<int>().ToList();
            List<string> reminder = new List<string>();
            foreach (var item in reminders)
            {
                reminder.Add(Enum.GetName(typeof(ReminderStatus), item));
            }
            return Ok(reminder);

        }
    }
}
