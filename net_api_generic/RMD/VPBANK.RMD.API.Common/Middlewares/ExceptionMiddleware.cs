using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using VPBANK.RMD.API.Common.Helpers.Errors;

namespace VPBANK.RMD.API.Common.Middlewares
{
    public class HttpErrorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Code { get; set; }
        public string ContentType { get; set; } = MediaTypeNames.Text.Plain;

        public HttpErrorException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpErrorException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpErrorException(HttpStatusCode statusCode, string errorCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
            this.Code = errorCode;
        }

        public HttpErrorException(HttpStatusCode statusCode, Exception inner) : this(statusCode, inner.ToString())
        {
        }

        public HttpErrorException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = MediaTypeNames.Application.Json;
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (HttpErrorException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpErrorException exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            if (exception is HttpErrorException)
            {
                var errorResult = new BusinessException() 
                { 
                    Timestamp = DateTime.Now, 
                    Status = exception.StatusCode, 
                    Error = exception.StackTrace, 
                    Message = exception.Message, 
                    Path = exception.Source
                };
                context.Response.StatusCode = (int)exception.StatusCode;
                return context.Response.WriteAsync(errorResult.ToString());
            }
            else
            {
                var errorResult = new BusinessException()
                {
                    Timestamp = DateTime.Now,
                    Status = HttpStatusCode.BadRequest,
                    Error = exception.StackTrace,
                    Message = exception.Message,
                    Path = exception.Source
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(errorResult.ToString());
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            if (!string.IsNullOrEmpty(exception.Message) && exception.Message.Equals("invalid signature.", StringComparison.CurrentCultureIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var errorResult = new BusinessException()
                {
                    Timestamp = DateTime.Now,
                    Status = HttpStatusCode.Unauthorized,
                    Error = exception.StackTrace,
                    Message = exception.Message,
                    Path = exception.Source
                };

                return context.Response.WriteAsync(errorResult.ToString());
            }
            else
            {
                var errorResult = new BusinessException()
                {
                    Timestamp = DateTime.Now,
                    Status = HttpStatusCode.InternalServerError,
                    Error = exception.StackTrace,
                    Message = exception.Message,
                    Path = exception.Source
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(errorResult.ToString());
            }
        }
    }
}
