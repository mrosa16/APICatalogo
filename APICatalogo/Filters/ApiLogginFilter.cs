using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger<ApiLogginFilter> _logger;

        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Executa antes do Action
            _logger.LogInformation("###Executando -> OnActionExecution");
            _logger.LogInformation("#################################");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("#################################");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Executa depois do Action
            _logger.LogInformation("###Executando -> OnActionExecuted");
            _logger.LogInformation("#################################");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation($"ModelState : {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("#################################");
            throw new NotImplementedException();
        }
    }
}
