using System.Text.Json;

namespace GpioController.Middlware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex) when (ex is Exception)
        {
            var response = context.Response;
            response.StatusCode = 500;
            var message = JsonSerializer.Serialize(new
            {
                code = 500,
                message = ex.Message
            });
            await response.WriteAsync(message);
        }
        catch (Exception ex) when (ex is Exception)
        {
            var response = context.Response;
            response.StatusCode = 404;
            var message = JsonSerializer.Serialize(new
            {
                code = 404,
                message = ex.Message
            });
            await response.WriteAsync(message);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.StatusCode = 500;
            var message = JsonSerializer.Serialize(new
            {
                code = 500,
                message = ex.Message
            });
            await response.WriteAsync(message);
        }
    }
}