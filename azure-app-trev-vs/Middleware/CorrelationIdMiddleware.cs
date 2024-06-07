using Microsoft.Extensions.Primitives;
using Serilog.Context;
using Serilog.Enrichers;

namespace azure_app_trev_vs.Middleware
{
    public class CorrelationIdMiddleware : IMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private const string CorrelationIdPropertyName = "CorrelationId";

        private static readonly string CorrelationIdItemName = typeof(CorrelationIdEnricher).Name + "+CorrelationId";
        public CorrelationIdMiddleware()
        {
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = context.Items[CorrelationIdItemName]?.ToString();
                if (StringValues.IsNullOrEmpty(correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                }
            }
            context.TraceIdentifier = correlationId!;
            context.Items[CorrelationIdItemName] = correlationId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(CorrelationIdHeader, correlationId);
                return Task.CompletedTask;
            });

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await next(context);
            }
        }
    }

}
