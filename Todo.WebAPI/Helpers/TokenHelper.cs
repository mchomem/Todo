using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.Core.Models.Dtos;

namespace Todo.WebAPI.Helpers
{
    public static class TokenHelper
    {
        public static string Generate(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("+WsLhdwMcCnW&cJW4a5hm^jFemE&?V?Y?z9eMdcN_X3DktLE7W9nS#Z2&vpakM6v");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"))
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
