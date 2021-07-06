using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT_Authentication
{
    public interface IJwtAuthenticationManager
    {
        AuthenticationResponse Authenticate(string username, string password);
        IDictionary<string, string> UsersRefreshToken { get; set; }
        AuthenticationResponse Authenticate(string username, Claim[] claims);
    }
}
