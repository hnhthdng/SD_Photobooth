using System.Diagnostics;

namespace ServerAPI.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;
                context.Response.Headers["X-Response-Time-ms"] = responseTime.ToString();
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
