using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class MailSettings
    {
        public string Email { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTP { get; set; }
    }
}
