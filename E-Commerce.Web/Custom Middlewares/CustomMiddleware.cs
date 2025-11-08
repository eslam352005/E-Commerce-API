using Domain.Exceptions;
using Shared.Model_Errors;

namespace E_Commerce.Web.Custom_Middlewares
{
    public class CustomMiddleware(RequestDelegate _next, ILogger<CustomMiddleware> _logger)
    {

        public async Task InvokeAsync(HttpContext context)
        {

			try
            {
                // Request

                await _next.Invoke(context);

                // Response
                await HandleNotFoundEndPoint(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went Wrong");

                await HandleExceptionAsync(context, ex);
            }

        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            var response = new ErrorToReturn()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message,
                Errors = ex switch
                {
                    BadRequestException badRequestException => badRequestException.Errors,
                    _ => []
                }
            };
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleNotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {context.Request.Path} is Not Found"
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
