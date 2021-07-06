using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            {"admin","12345" },
            {"system","namage" }
        };
        public IDictionary<string, string> UsersRefreshToken { get; set; }
        private readonly string key;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        //IDictionary<string, string> IJwtAuthenticationManager.UsersRefreshToken => throw new NotImplementedException();

        public JwtAuthenticationManager(string key,IRefreshTokenGenerator refreshTokenGenerator)
        {
            this.key = key;
            this.refreshTokenGenerator = refreshTokenGenerator;
            UsersRefreshToken = new Dictionary<string, string>();
        }
        public AuthenticationResponse Authenticate(string username,Claim [] claims)
        {
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var jwtSecuriryToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                );
            var token=new JwtSecurityTokenHandler().WriteToken(jwtSecuriryToken);
            var refreshToken = refreshTokenGenerator.TokenGenerator();
            if (UsersRefreshToken.ContainsKey(username))
            {
                UsersRefreshToken[username] = refreshToken;
            }
            else
            {
                UsersRefreshToken.Add(username, refreshToken);
            }
            
            return new AuthenticationResponse
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }
        public AuthenticationResponse Authenticate(string username, string password)
        {
            if(!users.Any(x=>x.Key==username && x.Value == password))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = refreshTokenGenerator.TokenGenerator();
            if (UsersRefreshToken.ContainsKey(username))
            {
                UsersRefreshToken[username] = refreshToken;
            }
            else
            {
                UsersRefreshToken.Add(username, refreshToken);
            }
            return new AuthenticationResponse
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };
        }
    }
}
