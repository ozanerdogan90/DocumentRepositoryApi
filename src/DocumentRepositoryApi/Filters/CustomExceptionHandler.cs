using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DocumentRepositoryApi.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;


        public CustomExceptionFilter(
                IHostingEnvironment hostingEnvironment,
                IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var isDev = _hostingEnvironment.IsDevelopment();

            var error = new ErrorModel()
            {
                Message = isDev ? exception.Demystify().ToString() : exception.Message,
                StackTrace = isDev ? exception.StackTrace.Split('\n') : null
            };

            if (exception.InnerException != null && isDev)
            {
                error.InnnerException = new ErrorModel()
                {
                    Message = exception.InnerException.Message,
                    StackTrace = exception.InnerException.StackTrace.Split('\n')
                };
            }

            context.Result = new ObjectResult(error)
            { StatusCode = (int)HttpStatusCode.InternalServerError };
        }

        private class ErrorModel
        {
            public string Message { get; set; }
            public string[] StackTrace { get; set; }
            public ErrorModel InnnerException { get; set; }
        }
    }
}
