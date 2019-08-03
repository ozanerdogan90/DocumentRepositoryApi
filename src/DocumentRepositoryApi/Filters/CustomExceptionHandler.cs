using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;

namespace DocumentRepositoryApi.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(
                IHostingEnvironment hostingEnvironment,
                IModelMetadataProvider modelMetadataProvider,
                ILogger<CustomExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
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

            var correlationId = context.HttpContext.TraceIdentifier;
            _logger.LogError(exception, exception.Message, correlationId);
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
