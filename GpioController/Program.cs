using GpioController.Authorization;
using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Factories;
using GpioController.Middlware;
using GpioController.Models;
using GpioController.Parsers;
using GpioController.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace GpioController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<AuthorizationSettings>(
            builder.Configuration.GetSection("Authorization")
        );
        
        builder.Services.Configure<FilterSettings>(
            builder.Configuration.GetSection("Filters")
        );
        
        var requireAuth = builder.Configuration.GetValue<bool>("Authorization:Enabled");

        if (requireAuth)
        {
            var authorizedCorsOrigin = builder.Configuration
                .GetSection("Authorization:AuthorizedCorsOrigins")
                .Get<string[]>();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AuthorizedCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(authorizedCorsOrigin ?? [])
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "OPTIONS");
                });
            });
        }
        else
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AuthorizedCorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "OPTIONS");
                });
            });
        }

        builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("ConditionalPolicy", policy =>
                policy.Requirements.Add(new ConditionalAuthRequirement()));
        
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

        builder.Services.AddSingleton<IAuthorizationHandler, ConditionalJwtAuthorization>();
        builder.Services.AddTransient<ITerminalService, TerminalService>();
        builder.Services.AddTransient<IGpioService, GpioService>();
        builder.Services.AddSingleton<ITokenManagementService, TokenManagementService>();
        builder.Services.AddTransient<IStateService, StateService>();
        builder.Services.AddTransient<IParser<GpioInfoResult>, InfoParser>();
        builder.Services.AddTransient<IParser<GpioSetResult>, UpdateParser>();
        builder.Services.AddTransient<IParser<GpioReadResult>, ReadParser>();
        builder.Services.AddTransient<ICommand<GpioInfoRequest, GpioInfoResult>, GpioInfoCommand>();
        builder.Services.AddTransient<ICommand<GpioReadRequest, GpioReadResult>, GpioReadCommand>();
        builder.Services.AddTransient<ICommand<GpioSetRequest, GpioSetResult>, GpioSetCommand>();
        builder.Services.AddSingleton<ICommandFactory>(provider => new CommandFactory(provider));

        builder.Services.AddControllers();

        var app = builder.Build();
        
        app.UseCors("AuthorizedCorsPolicy");
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
        
        app.Run();
    }
}
