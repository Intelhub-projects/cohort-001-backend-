using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enums
{
    public enum RegistrationStatus
    {
        [Description("Registered")]
        Registered = 1,
        [Description("Verified")]
        Verified
    }
}
