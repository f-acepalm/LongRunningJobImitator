
using LongRunningJobImitator.Api;
using LongRunningJobImitator.Notification.Api.SignalR;

namespace LongRunningJobImitator.Notification.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddApiServices()
            .AddSignalR();

        builder.Services.AddCors(x =>
        {
            x.AddPolicy("CorsPolicy", options =>
            {
                options.WithOrigins(GetCorsAllowedOrigins(builder.Configuration))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<TextConversionHub>("/text-conversion-hub");

        app.Run();
    }

    private static string[] GetCorsAllowedOrigins(IConfiguration configuration)
     => (configuration.GetSection("CorsAllowedOrigins").Get<IEnumerable<string>>() ?? Enumerable.Empty<string>())
     .Distinct()
     .ToArray();
}
