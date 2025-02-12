using Microsoft.AspNetCore.Builder;
using VPBANK.RMD.API.Common.Middlewares;

namespace VPBANK.RMD.API.Common.Extensions
{
    public static class ErrorExceptionExtensions
    {
        public static void ConfigureErrorExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
