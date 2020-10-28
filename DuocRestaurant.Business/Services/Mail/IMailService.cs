using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IMailService
    {
        public void SendMail(string mailTo, string subject, string body);
    }
}
