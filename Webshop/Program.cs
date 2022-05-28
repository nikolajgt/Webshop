
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Webshop.Extensions.BackgroundWorkers;
using Webshop.Extensions.JWT;
using Webshop.Hubs;
using Webshop.Interface;
using Webshop.Interface.Generic;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Repositorys;
using Webshop.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddIdentityCore<Customer>(i =>
{
    i.Password.RequireDigit = false;
    i.Password.RequireNonAlphanumeric = false;
    i.Password.RequireUppercase = false;
    i.Password.RequiredLength = 2;
}).AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddIdentityCore<Admin>(i =>
{
    i.Password.RequireDigit = false;
    i.Password.RequireNonAlphanumeric = false;
    i.Password.RequireUppercase = false;
    i.Password.RequiredLength = 2;
}).AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddDbContextFactory<MyDbContext>(opt => opt
    .EnableDetailedErrors()
    .UseSqlServer(config.GetConnectionString("SqlServer")));

builder.Services.AddScoped(typeof(IGenericUserRepository<>), typeof(GenericUserRepository<>));
builder.Services.AddScoped(typeof(IGenericProductRepository<>), typeof(GenericProductRepository<>));

builder.Services.AddScoped<IShoppingService, ShoppingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJwtExtension, JwtExtension>();

builder.Services.AddHostedService<SimulateOrderDelivery>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Beaerer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.ApiKey,
    });


    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetSection("JWTkey").ToString())),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        x.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessTokenResponse = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (String.IsNullOrEmpty(accessTokenResponse))
                    return Task.CompletedTask;

                var accessTokenSkippedFirst = string.Join(string.Empty, accessTokenResponse.Skip(8));
                var finalAccessToken = accessTokenSkippedFirst.Remove(accessTokenSkippedFirst.Length - 1, 1);

                //var test = context.HttpContext.User;
                //var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(finalAccessToken))
                {
                    context.Token = finalAccessToken;
                }
                return Task.CompletedTask;
            }
        };
    }).AddCookie(c => c.SlidingExpiration = true);

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opt =>
    opt.MimeTypes = ResponseCompressionDefaults
    .MimeTypes
    .Concat(new[] { "application/octet-stream" })
);

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationsHub>("/hubs/NotificationsHub");

app.MapControllers();

app.Run();
