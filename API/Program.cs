// TODO: Add Exception & Logging Middleware

using API.Datas;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Scalar.AspNetCore;
using API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddScoped<IMutualFundService, MutualFundService>();
builder.Services.AddScoped<IMutualFundRepository, MutualFundRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsEnvironment("Testing")) builder.Services.AddDbContext<MFDbContext>(options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("Supabase")!));
else builder.Services.AddDbContext<MFDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("IpRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }
        )
    );
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionMiddleware>();

var app = builder.Build();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<LoggingMiddleware>();
//! prod use only app.UseStaticFiles();
//! prod use only app.UseHttpsRedirection(); 
app.UseCors();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();
app.Run();
