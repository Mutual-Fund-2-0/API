using API.Datas;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddScoped<IMutualFundService, MutualFundService>();
builder.Services.AddScoped<IMutualFundRepository, MutualFundRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsEnvironment("Testing")) builder.Services.AddDbContext<MFDbContext>(options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("Supabase")!));
else builder.Services.AddDbContext<MFDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//! prod use only app.UseStaticFiles();
//! prod use only app.UseHttpsRedirection(); 
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
