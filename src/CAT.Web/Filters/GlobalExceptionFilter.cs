using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CAT.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var errorMessage = "An unexpected server error occurred.";
            
            var response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";

            context.ExceptionHandled = true;
            context.Result = _env.IsDevelopment() ?
                new JsonResult(context.Exception.Message) :
                new JsonResult(errorMessage);
        }
    }
}
