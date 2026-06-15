using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gymly.Web.Filters;

public class FluentValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            foreach (var error in validationException.Errors)
            {
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            };

            context.Result = viewResult;
            context.ExceptionHandled = true;
        }
    }
}