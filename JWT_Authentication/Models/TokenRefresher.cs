using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication.Models
{
    public class TokenRefresher:ITokenRefresher
    {
        private readonly string key;
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public TokenRefresher(string key, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.key = key;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }
        public AuthenticationResponse Refresher(RefreshCred refresh)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var principle = tokenHandler.ValidateToken(refresh.JwtToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                },out validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;
            if(jwtToken==null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            var username = principle.Identity.Name;
            if (refresh.RefreshToken!= jwtAuthenticationManager.UsersRefreshToken[username])
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return jwtAuthenticationManager.Authenticate(username, principle.Claims.ToArray());


        }
    }
}
