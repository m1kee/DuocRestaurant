using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SendMailRequest
    {
        public string MailTo { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
    }
}
