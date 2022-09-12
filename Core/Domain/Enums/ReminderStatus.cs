using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enums
{
    public enum ReminderStatus
    {
        [Description("Onboard")]
        Onboard = 1,
        [Description("Done")]
        Done
    }
}
