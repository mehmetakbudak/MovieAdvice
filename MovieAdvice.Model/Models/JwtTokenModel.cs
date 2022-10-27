using System;

namespace MovieAdvice.Model.Models
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
