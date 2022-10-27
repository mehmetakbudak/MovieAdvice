using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Service.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Attributes
{
    internal class AuthorizeAttritube : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                var configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));

                var request = context.HttpContext.Request;
                var token = request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(token))
                {
                    throw new NotFoundException("Token bulunamadı.");
                }

                string tokenString = token.Split(' ')[1];

                if (string.IsNullOrEmpty(tokenString))
                {
                    throw new BadRequestException("Token formatı uygun değil.");
                }

                var handler = new JwtSecurityTokenHandler();
                var tokenDescrypt = handler.ReadJwtToken(tokenString);
                var secret = configuration.GetSection("Jwt:Secret").Value;
                var key = Encoding.ASCII.GetBytes(secret);

                if (!Int32.TryParse(tokenDescrypt.Payload["UserId"].ToString(), out int userId))
                {
                    throw new BadRequestException("Token hatalı.");
                }

                try
                {
                    handler.ValidateToken(tokenString, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    }, out SecurityToken securityToken);
                }
                catch
                {
                    throw new BadRequestException("Token doğrulanamadı.");
                }

                var user = await userService.GetById(userId);

                if (user == null)
                {
                    throw new NotFoundException("Kullanıcı bulunamadı.");
                }

                if (!string.IsNullOrEmpty(user.Token))
                {
                    if (user.Token.ToLower() != tokenString.ToLower())
                    {
                        throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                    }
                }
                else
                {
                    throw new UnAuthorizedException("Lütfen giriş yapınız.");
                }

                if (!user.TokenExpireDate.HasValue)
                {
                    throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                }

                var tokenStartDate = user.TokenExpireDate.Value.AddHours(-2);
                var tokenEndDate = user.TokenExpireDate.Value;

                if (!((tokenStartDate <= DateTime.Now) && (tokenEndDate >= DateTime.Now)))
                {
                    throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                }
            }
            catch (Exception ex)
            {
                throw new UnAuthorizedException(ex.Message);
            }
        }
    }
}
