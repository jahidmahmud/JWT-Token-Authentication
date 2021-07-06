using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JWT_Authentication
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string TokenGenerator()
        {
            var randomNumber = new Byte[32];
            using (var randomNumberGenerator=RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
