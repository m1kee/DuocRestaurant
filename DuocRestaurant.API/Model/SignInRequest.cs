using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SignInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
