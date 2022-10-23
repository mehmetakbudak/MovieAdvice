using System;
using System.Net;
using System.Text;
using MovieAdvice.Model.Models;
using MovieAdvice.Service.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace MovieAdvice.Service.Attributes
{
    internal class AuthorizeAttritube : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                var configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));

                var request = context.HttpContext.Request;
                var token = request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token bulunamadı.");
                }
                string tokenString = token.Split(' ')[1];
                if (string.IsNullOrEmpty(tokenString))
                {
                    throw new Exception("Token formatı uygun değil.");
                }
                var handler = new JwtSecurityTokenHandler();
                var tokenDescrypt = handler.ReadJwtToken(tokenString);
                var secret = configuration.GetSection("Jwt:Secret").Value;
                var key = Encoding.ASCII.GetBytes(secret);

                if (Int32.TryParse(tokenDescrypt.Payload["UserId"].ToString(), out int userId))
                {
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
                        throw new Exception("Token doğrulanamadı.");
                    }

                    var user = userService.GetById(userId);

                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.Token))
                        {
                            if (user.Token.ToLower() != tokenString.ToLower())
                            {
                                throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                            }
                        }
                        else
                        {
                            throw new Exception("Lütfen giriş yapınız.");
                        }
                        if (user.TokenExpireDate.HasValue)
                        {
                            AuthTokenContent authTokenContent = new AuthTokenContent();
                            AuthTokenContent.Current = authTokenContent;

                            var tokenStartDate = user.TokenExpireDate.Value.AddHours(-2);
                            var tokenEndDate = user.TokenExpireDate.Value;

                            AuthTokenContent.Current.UserId = user.Id;

                            if (!((tokenStartDate <= DateTime.Now) && (tokenEndDate >= DateTime.Now)))
                            {
                                throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                            }
                        }
                        else
                        {
                            throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                        }
                    }
                    else
                    {
                        context.Result = new NotFoundObjectResult(new ServiceResult
                        {
                            Message = "Kullanıcı bulunamadı.",
                            StatusCode = HttpStatusCode.NotFound
                        });
                    }
                }
                else
                {
                    throw new Exception("Token hatalı.");
                }
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new ServiceResult
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.Unauthorized
                });
            }
        }
    }
}
