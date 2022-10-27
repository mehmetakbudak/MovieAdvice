using MovieAdvice.Model.Models;
using System;
using System.Net;

namespace MovieAdvice.Infrastructure.Exceptions
{
    public abstract class ApiExceptionBase : Exception
    {
        public ApiExceptionBase()
        {
        }

        public ApiExceptionBase(string message) : base(message)
        {

        }

        public BaseResult Error { get; set; }

        protected abstract HttpStatusCode HttpStatusCode { get; }

        public int StatusCode => (int)HttpStatusCode;
    }
}
