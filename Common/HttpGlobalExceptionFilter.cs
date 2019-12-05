namespace RestaurantApplication.Api.Common
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using RestaurantApplication.Api.Logging;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = false;
            Logger.Log(context);
        }

        public void Dispose()
        {

        }
    }
}
