using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Profiles;
using Services.Exceptions;
using System;
using System.Net.Http;
using System.Security.Claims;

namespace Game.Server.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddIdentityContextConfig(this IServiceCollection services)
        {
            services.AddScoped<IIdentityContext, IdentityContext>(sp =>
            {
                var httpContext = sp.GetService<IHttpContextAccessor>().HttpContext;

                var identityContext = new IdentityContext();

                if (httpContext == null)
                    return identityContext;

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

        public static IServiceCollection AddTokenConfig(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

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
                config.AddProfile(new ReportProfile());
                config.AddProfile(new QueueProfile());
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
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();


            services.AddScoped<IClaimsTransformation, MyUserClaimsTransformer>();


            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                {
                    options.IdentityResources["openid"].UserClaims.Add("role"); // Roles  
                    options.IdentityResources["openid"].UserClaims.Add(ClaimTypes.NameIdentifier);
                    options.IdentityResources["openid"].UserClaims.Add(ClaimTypes.Name);
                    options.IdentityResources["openid"].UserClaims.Add(ClaimTypes.Email);
                });

            services.AddAuthentication()
               .AddIdentityServerJwt();

            return services;
        }


    }
}
