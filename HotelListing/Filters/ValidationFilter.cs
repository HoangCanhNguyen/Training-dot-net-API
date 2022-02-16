using HotelListing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
                var errorResponse = new ErrorReponse();

                foreach (var error in errors)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorDetail = new Error
                        {
                            Key = error.Key,
                            Message = subError
                        };

                        errorResponse.Errors.Add(errorDetail);
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            await next();
        }
    }
}
