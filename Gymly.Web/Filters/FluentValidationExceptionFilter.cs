using FluentValidation;
using Gymly.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gymly.Web.Filters;

public class FluentValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        string? errorMessage = null;

        if (context.Exception is GymlyValidationException gymlyEx)
        {
            errorMessage = gymlyEx.Message;
        }

        if (errorMessage == null) return;

        var tempDataFactory = context.HttpContext.RequestServices
            .GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
        var tempData = tempDataFactory?.GetTempData(context.HttpContext);
        if (tempData != null)
        {
            tempData["ErrorMessage"] = errorMessage;
        }

        var controller = context.RouteData.Values["controller"]?.ToString() ?? "Home";
        var action = context.RouteData.Values["action"]?.ToString() ?? "Index";

        context.Result = new RedirectToActionResult(action, controller, null);
        context.ExceptionHandled = true;
    }
}