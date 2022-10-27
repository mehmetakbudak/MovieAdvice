﻿using MovieAdvice.Model.Models;
using System.Net;

namespace MovieAdvice.Infrastructure.Exceptions
{
    public class NotAcceptableException : ApiExceptionBase
    {
        public NotAcceptableException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotAcceptable, Message = "Kabul Edilmeyen İstek." };
        }

        public NotAcceptableException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotAcceptable, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotAcceptable;
    }
}
