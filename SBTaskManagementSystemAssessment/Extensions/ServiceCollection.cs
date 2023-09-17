using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Infrastructure;
using SBTaskManagement.Infrastructure.DBContext;
using SBTaskManagementSystemAssessment.Middlewares;
using System.Reflection;
using FluentValidation;
using System.Text;

namespace SBTaskManagementSystemAssessment.Extensions
{
    public static class ServiceCollection
    {
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.RegisterServices();
            services.AddClientDbContext(config);
            services.ConfigureAutoMapper();
            services.RegisterExternalServices();
            services.ConfigureSwagger();
            services.ConfigureJwt(config);
            services.ConfigureOptions(config);
            services.ConfigureCors();

           
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<JwtConfigSettings>().BindConfiguration("JwtSettings");
            services.AddOptions<MailSettings>().BindConfiguration("MailSettings");
            services.AddOptions<AppConfig>().BindConfiguration("appConfig");
         
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opts =>
            {
                opts.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "@StackBuld Assessment Task",
                    Version = "v1",
                    Description = "Task Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "snipercadet",
                        Email = "donibris@gmail.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });


               
            });
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("ValidIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("ValidAudience").Value,
                    IssuerSigningKey = new
                        SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtUserSecret))
                };
            });
        }

    }
}
