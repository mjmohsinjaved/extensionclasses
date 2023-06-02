using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SharedLibrary.HttpCLientExtension
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Content { get; set; }
        public string? Message { get; set; }
        public T? ResponseObject { get; set; }
    }
    public class ApiResponse : ApiResponse<object>
    {

    }
}
