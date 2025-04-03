using System.Text.Json;
using GpioController.Exceptions;

namespace GpioController.Middlware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex) when (ex is InvalidStateException)
        {
            var response = context.Response;
            response.StatusCode = 400;
            var message = JsonSerializer.Serialize(new
            {
                code = 400,
                message = ex.Message
            });
            await response.WriteAsync(message);
        }
        catch (Exception ex) when (ex is GpioNotFoundException or NoGpiosFoundException)
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