using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string HtmlContent { get; set; }
    }
}
