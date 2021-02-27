﻿using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Models.Profiles;
using Repositories.Implementations;
using Services.Exceptions;
using Services.Implementations;
using Services.Implementations.AttackService;
using Services.Implementations.BuildingService;
using Services.Interfaces;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Game.Server.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddMyServices(
             this IServiceCollection services)
        {
            services.AddScoped<BackEnd.Services.Interfaces.IAuthenticationService, BackEnd.Services.Implementations.AuthenticationService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IAttackService, AttackService>();

            return services;
        }

        public static IServiceCollection AddIdentityContextConfig(this IServiceCollection services) 
        {
            services.AddScoped<IIdentityContext, IdentityContext>(sp => 
            {
                var httpContext = sp.GetService<IHttpContextAccessor>().HttpContext;

                var identityContext = new IdentityContext();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    string id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    string username = httpContext.User.FindFirst(ClaimTypes.Name).Value;
                    string email = httpContext.User.FindFirst(ClaimTypes.Email).Value;
                    bool isAdmin = "Admin".Equals(httpContext.User.FindFirst(ClaimTypes.Role).Value);

                    identityContext.UserId = id;
                    identityContext.Username = username;
                    identityContext.Email = email;
                    identityContext.IsAdmin = isAdmin;
                }
                return identityContext;
            });

            return services;
        }


        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerJwt()
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidAudience = configuration["AuthenticationSettings:Audience"],
                     ValidIssuer = configuration["AuthenticationSettings:Issuer"],
                     RequireExpirationTime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthenticationSettings:Key"])),
                     ValidateIssuerSigningKey = true
                 };
             });

            return services;
        }


        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services) 
        {
            services.AddSingleton(sp => new AutoMapper.MapperConfiguration(config =>
            {
                config.AddProfile(new UserCredentialProfile());
                config.AddProfile(new BuildingUpgradeCostProfile());
                config.AddProfile(new ResourceMapper());
                config.AddProfile(new UnitProfile());
                config.AddProfile(new CityProfile());
            }).CreateMapper());

            return services;
        }

        public static IServiceCollection AddProblemDetailsConfig(this IServiceCollection services) 
        {
            services.AddProblemDetails(opt =>
            {
                opt.IncludeExceptionDetails = (context, ex) =>
                {
                    var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();

                    return environment.IsDevelopment();
                };
                // This will map NotImplementedException to the 501 Not Implemented status code.
                opt.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

                //Mapping my custom exceptions to status codes
                opt.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
                opt.MapToStatusCode<UnAuthorizedUserException>(StatusCodes.Status401Unauthorized);
                opt.MapToStatusCode<BannedUserException>(StatusCodes.Status403Forbidden);
                opt.MapToStatusCode<NotConfirmedAccountException>(StatusCodes.Status422UnprocessableEntity);
                opt.MapToStatusCode<OperationFailedException>(StatusCodes.Status500InternalServerError);
                opt.MapToStatusCode<BadRequestException>(StatusCodes.Status400BadRequest);

                // This will map HttpRequestException to the 503 Service Unavailable status code.
                opt.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });

            return services;
        }

        public static IServiceCollection AddIdentityConfig(this IServiceCollection services) 
        {
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            }).AddRoles<IdentityRole>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<DeletionTokenProvider<ApplicationUser>>("AccountDeletion")
              .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            return services;
        }
    }
}
