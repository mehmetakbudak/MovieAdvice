﻿using System.Net;

namespace MovieAdvice.Model.Models
{
    public class BaseResult
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ServiceResult : BaseResult
    {
        public object Data { get; set; }
    }
}
