using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication
{
    public interface IRefreshTokenGenerator
    {
        string TokenGenerator();
    }
}
