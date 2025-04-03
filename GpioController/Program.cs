using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Factories;
using GpioController.Middlware;
using GpioController.Models;
using GpioController.Parsers;
using GpioController.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GpioController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<AuthorizationSettings>(
            builder.Configuration.GetSection("Authorization")
        );

        builder.Services.AddTransient<ITerminalService, TerminalService>();
        builder.Services.AddTransient<IGpioService, GpioService>();
        builder.Services.AddTransient<IParser<GpioInfoResult>, InfoParser>();
        builder.Services.AddTransient<IParser<GpioSetResult>, UpdateParser>();
        builder.Services.AddTransient<ICommand<GpioInfoRequest, GpioInfoResult>, GpioInfoCommand>();
        builder.Services.AddTransient<ICommand<GpioSetRequest, GpioSetResult>, GpioSetCommand>();
        builder.Services.AddTransient<ICommandFactory>(provider => new CommandFactory(provider));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://accounts.google.com";
                options.TokenValidationParameters = new()
                {
                    ValidIssuer = "https://accounts.google.com",
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
        
        app.Run();
    }
}
