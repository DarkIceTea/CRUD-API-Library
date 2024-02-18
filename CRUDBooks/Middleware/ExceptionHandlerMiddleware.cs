using CRUDBooks.Exceptions;
using Newtonsoft.Json;


namespace CRUDBooks.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new { error = ex.Message };
                var jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception ex)
            {
                // Обработка исключения
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                // Формирование ответа с информацией об ошибке
                var response = new { error = ex.Message };
                var jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
