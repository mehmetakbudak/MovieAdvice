using MovieAdvice.Storage.Consts;
using System;
using System.Linq;

namespace MovieAdvice.Infrastructure.Extensions
{
    public static class ClaimsExtensions
    {
        public static int? UserId(this System.Security.Claims.ClaimsPrincipal principal)
        {
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == JwtTokenPayload.UserId);

            if (userIdClaim == null || !Int32.TryParse(userIdClaim.Value, out int userId))
            {
                return null;
            }

            return userId;
        }
    }
}
