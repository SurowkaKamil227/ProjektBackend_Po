using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace   WebAPI.Middleware
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Dodawanie niestandardowego nagłówka do odpowiedzi
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("X-Custom-Header", "Header Value");
                return Task.CompletedTask;
            });

            // Kontynuacja przetwarzania żądania
            await _next(context);
        }
    }
}
