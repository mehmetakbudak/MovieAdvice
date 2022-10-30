using MovieAdvice.Storage.Models;
using System.Net;

namespace MovieAdvice.Infrastructure.Exceptions
{
    public class UnAuthorizedException : ApiExceptionBase
    {
        public UnAuthorizedException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Unauthorized, Message = "Kimlik bilgileri doğrulanamadı." };
        }

        public UnAuthorizedException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Unauthorized, Message = message };
        }


        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Unauthorized;
    }
}
