using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using VPBANK.RMD.API.Common.Helpers.Requests;

namespace VPBANK.RMD.API.Common.Filters
{
    public class AsyncActionFilterHandler : IAsyncActionFilter
    {
        protected readonly IConfiguration _configuration;
        protected readonly RequestHandler _requestHandler;

        public AsyncActionFilterHandler(IConfiguration configuration,
            RequestHandler requestHandler)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            var resultContext = await next();
        }
    }
}
