using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication.Models
{
    public class RefreshCred
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
