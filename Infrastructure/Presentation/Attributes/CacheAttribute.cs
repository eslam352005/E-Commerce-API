using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int durationInSec = 120) : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          
            string cacheKey = CreateCacheKey(context.HttpContext.Request);
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await cacheService.GetCacheAsync(cacheKey);

            if (cacheValue != null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                };
                return;
            }

            var ExecutedContext = await next.Invoke();

            if( ExecutedContext.Result is OkObjectResult result){

                await cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromSeconds(durationInSec));
            }

        }


        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q=> q.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }
            return key.ToString();
        }
    }
}
