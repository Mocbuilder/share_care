using Microsoft.AspNetCore.Authentication;
using Npgsql;
using sharecare_backend.Filters;
using sharecare_backend.Services;

namespace sharecare_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            builder.Services.AddNpgsqlDataSource(connectionString);

            builder.Services.AddScoped<DbService>();

            var corsOrigin = builder.Configuration.GetValue<string>("CorsOrigin")
                 ?? "https://your-allowed-origin.com";

            // 1. Add CORS services and define the policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CustomCorsPolicy", policy =>
                {
                    policy.WithOrigins(corsOrigin)
                          .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                          .WithHeaders("Authorization", "Content-Type", "Access-Control-Allow-Origin");
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
    
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbService = scope.ServiceProvider.GetRequiredService<DbService>();
                dbService.CheckAndCreateTablesAsync().GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("CustomCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
