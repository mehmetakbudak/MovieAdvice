using System;

namespace MovieAdvice.Storage.Models
{
    public class TokenReponseModel
    {
        public int UserId { get; set; }
        public string NameSurname { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}
