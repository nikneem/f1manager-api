using F1Manager.Shared.Base;
using F1Manager.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F1Manager.Api.Filters
{

    public class F1ManagerExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is F1ManagerException exception)
            {
                context.Result = new ObjectResult(new ErrorMessageDto
                {
                    ErrorCode = exception.ErrorCode.Code,
                    TranslationKey = exception.ErrorCode.TranslationKey,
                    ErrorMessage = exception.Message,
                    Substitutions = exception.Substitutes
                })
                {
                    StatusCode = 409
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
