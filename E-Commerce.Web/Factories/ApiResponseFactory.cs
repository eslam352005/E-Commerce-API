using Microsoft.AspNetCore.Mvc;
using Shared.Model_Errors;

namespace E_Commerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrors(ActionContext context)
        {
            var errors = context.ModelState.Where(m => m.Value.Errors.Any())
                                       .Select(m => new ValidationError()
                                       {
                                           Field = m.Key,
                                           Errors = m.Value.Errors.Select(m => m.ErrorMessage)
                                       });
            var response = new ValidationsErrorsToReturn()
            {
                ValidationErrors = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
