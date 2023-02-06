using List_Domain.Exeptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MySecondProject.Filters
{
    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute // шо за назва така?  воно взагалі працює?
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (context.Exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(new { errorMessage = context.Exception.Message });
            }
            else if (context.Exception is ValidProblemException)
            {
                context.Result = new BadRequestObjectResult(new { errorMessage = context.Exception.Message });
            }
        }
    }
}
