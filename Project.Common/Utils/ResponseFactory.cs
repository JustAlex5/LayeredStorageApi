using Project.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Utils
{
    public static class ResponseFactory
    {
        public static ResponseModel<T> Success<T>(T data, string message = "OK", int statusCode = 200) =>
            new() { StatusCode = statusCode, Message = message, Data = data, IsError = false };

        public static ResponseModel<T> Error<T>(string message, int statusCode = 400) =>
            new() { StatusCode = statusCode, Message = message, IsError = true };
    }
}
