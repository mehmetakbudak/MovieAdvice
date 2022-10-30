﻿using MovieAdvice.Storage.Models;
using System.Net;

namespace MovieAdvice.Infrastructure.Exceptions
{
    public class FoundException : ApiExceptionBase
    {
        public FoundException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Found, Message = "Kayıt Mevcut." };
        }

        public FoundException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Found, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Found;
    }
}
