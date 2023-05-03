using RestaurantManager.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantManager.Repositories.Interfaces;
using RestaurantManager.Repositories;
using BookMySeatApi.Services;
using RestaurantManager.Observer;
using RestaurantManager.Services;
using RestaurantManager.Mappings;
using AutoMapper;
using RestaurantManager.Tests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IClientRepo, ClientRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IObserver, Logger>();
builder.Services.AddScoped<IObserver, EmailSender>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(OrderMapping));

builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddSingleton<PasswordService>();

builder.Services.AddScoped<Tester>();

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
