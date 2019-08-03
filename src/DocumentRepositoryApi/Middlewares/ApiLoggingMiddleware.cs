using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ApiLoggingMiddleware> _logger;
        private const string errorFormat = "Time:{0}   StatusCode:[{1}]    ResponseTime:[{2}]ms   Method:[{3}] Url:[{4}][{5}] Cid:[8] Payload:[{6}]   Response:[{7}]";
        private const string infoFormat = "Time:{0} ResponseTime:[{1}]ms   Method:[{2}] Url:[{3}][{4}]";
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private const int ReadChunkBufferLength = 4096;

        public ApiLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context, ILogger<ApiLoggingMiddleware> logger)
        {
            _logger = logger;
            var request = context.Request;
            var model = new RequestProfilerModel
            {
                RequestTime = DateTime.Now,
                Method = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                RequestContent = await GetRequestBody(request),
            };

            context.Response.OnCompleted(async () =>
            {
                context.Response.Headers.TryGetValue("X-Correlation-ID", out StringValues colId);
                model.CorrelationId = colId.ToString();
                Log(model);

            });

            Stream originalBody = context.Response.Body;

            using (MemoryStream newResponseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = newResponseBody;
                var sb = new Stopwatch();
                sb.Start();
                await _next(context);
                sb.Stop();
                model.ResponseTime = sb.ElapsedMilliseconds;
                model.StatusCode = context.Response.StatusCode;

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalBody);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                model.ResponseContent = ReadStreamInChunks(newResponseBody);

            }
        }



        public async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            request.EnableRewind();
            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await request.Body.CopyToAsync(requestStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                return ReadStreamInChunks(requestStream);
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;
                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

        public void Log(RequestProfilerModel model)
        {
            if (model.StatusCode >= 300 || model.StatusCode < 200)
            {
                _logger.LogError(string.Format(errorFormat, model.RequestTime, model.StatusCode, model.ResponseTime, model.Method, model.Path, model.QueryString, model.RequestContent, model.ResponseContent, model.CorrelationId));
            }
            else
            {
                _logger.LogInformation(string.Format(infoFormat, model.RequestTime, model.ResponseTime, model.Method, model.Path, model.QueryString));
            }
        }

        public class RequestProfilerModel
        {
            public DateTime RequestTime { get; set; }
            public double ResponseTime { get; set; }
            public string RequestContent { get; set; }
            public string ResponseContent { get; set; }
            public int StatusCode { get; set; }
            public string Method { get; set; }
            public string Path { get; set; }
            public string QueryString { get; set; }
            public string CorrelationId { get; set; }
        }
    }
}