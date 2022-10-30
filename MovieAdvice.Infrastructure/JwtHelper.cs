using Microsoft.IdentityModel.Tokens;
using MovieAdvice.Infrastructure.Configurations;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Storage.Consts;
using MovieAdvice.Storage.Entities;
using MovieAdvice.Storage.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAdvice.Infrastructure
{
    public interface IJwtHelper
    {
        JwtTokenModel GenerateJwtToken(User user);
        JwtSecurityToken ValidateToken(string token);
    }

    public class JwtHelper : IJwtHelper
    {
        public JwtTokenModel GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(WebApiConfiguration.Secret);
            var expireDate = DateTime.Now.AddHours(2);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtTokenPayload.UserId, user.Id.ToString()),
                    new Claim(JwtTokenPayload.FirstName, user.FirstName),
                    new Claim(JwtTokenPayload.LastName, user.LastName),
                    new Claim(JwtTokenPayload.Email, user.Email)
                }),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var model = new JwtTokenModel
            {
                Token = token,
                ExpireDate = expireDate
            };
            return model;
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            try
            {
                JwtSecurityToken jwtToken = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(WebApiConfiguration.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken != null)
                {
                    jwtToken = (JwtSecurityToken)validatedToken;
                }
                else
                {
                    throw new BadRequestException("Token doğrulanamadı.");
                }
                return jwtToken;
            }
            catch (Exception)
            {
                throw new UnAuthorizedException("Token doğrulanamadı.");
            }
        }
    }
}
