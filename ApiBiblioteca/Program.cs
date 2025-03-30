using ApiBiblioteca.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy =>
    {
        policy.WithOrigins("https://localhost:7223") 
              .AllowAnyHeader()
              .AllowAnyMethod();


    });
});


var _connectionStrings = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AplicationDbContext>(
           options => options.UseMySql(_connectionStrings, ServerVersion.AutoDetect(_connectionStrings))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("cors");

app.MapControllers();

app.Run();
