﻿using MovieAdvice.Model.Models;
using System;
using System.Net;

namespace MovieAdvice.Infrastructure.Exceptions
{
    public class NotFoundException : ApiExceptionBase
    {
        public NotFoundException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotFound, Message = "Kayıt bulunamadı." };
        }

        public NotFoundException(String message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotFound, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
    }
}
