using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.Extensions.Options;
using ModelLayer.Dto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<BookStoreContext>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IAuthServiceRL, AuthServiceRL>();

builder.Services.AddScoped<IEmailServiceBL, EmailServiceBL>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailServiceRL, EmailServiceRL>();

// Change the injection to use IOptions<EmailSettings>
builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
