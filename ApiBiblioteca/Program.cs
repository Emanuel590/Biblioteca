using System.Text;
using ApiBiblioteca.Data;
using ApiBiblioteca.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

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
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddSingleton<JwtService>();



builder.Services.Configure<FormOptions>(options =>
{
 
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024;   // Establece el límite a 50 MB, o ajusta según tus necesidades
});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "img");

// Si la carpeta no existe, se crea
if (!Directory.Exists(imgPath))
{
    Directory.CreateDirectory(imgPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imgPath),
    RequestPath = "/img"
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("cors");

app.MapControllers();

app.Run();
