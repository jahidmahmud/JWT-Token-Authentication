using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication.Models
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresher(RefreshCred refresh);
    }
}
