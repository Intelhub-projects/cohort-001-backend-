﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.DTOs
{
    public class MailRequestDto
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string HtmlContent { get; set; }
    }
}
