
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Abstractions;
using Middle.Middlewares;
using System.Threading.RateLimiting;

namespace Middle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ExceptionMiddleware>();
            builder.Services.AddControllers();
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", configure =>
                {
                    configure.Window = TimeSpan.FromSeconds(1);// kaç saniyede istek aatcak
                    configure.PermitLimit = 100; // kaç istek kabul edecek
                    configure.QueueLimit = 100;// istek dýsýnda kalanlarýn kaçý kuyruga eklenecek
                    configure.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // iþlenme sýrasý fifo mantýgý
                });
            });
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks().AddCheck("healthcheck",()=>HealthCheckResult.Healthy());
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseHealthChecks("healthcheck");
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers().RequireRateLimiting("fixed"); // butun kontrolllerda rate limit yapar
            app.UseRateLimiter();
            app.Run();
        }
    }
}
