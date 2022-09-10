using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enums
{
    public enum ReminderType
    {
        [Description("Daily")]
        Daily = 1,
        [Description("NonDaily")]
        NonDaily
    }
}
