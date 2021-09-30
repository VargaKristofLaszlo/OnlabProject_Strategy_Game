using System;
using System.Linq;
using System.Security.Claims;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Implementations;
using BackEnd.Repositories.Interfaces;
using Game.Server;
using Game.Server.Extensions;
using Game.Test.Helpers;
using Game.Test.Mock;
using Hangfire;
using Hangfire.MediatR;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using Services.Implementations;
using Services.Interfaces;
using Services.Queries;

namespace Game.Test.WebApplicationFactory
{
    public class TestingWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public ServiceProvider ServiceProvider { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);



                // Add Hangfire services.
                services.AddHangfire(configuration =>
                {
                    configuration
                                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                .UseSimpleAssemblyNameTypeSerializer()
                                .UseRecommendedSerializerSettings()
                                .UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HangfireTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                                    new SqlServerStorageOptions
                                    {
                                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                        QueuePollInterval = TimeSpan.Zero,
                                        UseRecommendedIsolationLevel = true,
                                        DisableGlobalLocks = true
                                    });
                    configuration.UseMediatR();
                });

                // Add the processing server as IHostedService
                services.AddHangfireServer();

                services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(
                         "Data Source=DESKTOP-3PC4U44;Initial Catalog=StrategyGameTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                         sqlOptions =>
                         {
                             sqlOptions.MigrationsAssembly("Game.Server");
                         }));

                services.AddDatabaseDeveloperPageExceptionFilter();

                services.AddScoped<IUnitOfWork, EfUnitOfWork>();

                //.........................
                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = true;
                }).AddRoles<IdentityRole>()
                  .AddDefaultTokenProviders()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();
                services.AddScoped<IClaimsTransformation, MyUserClaimsTransformer>();
                services.AddIdentityServer();
                //..........................

                services.AddAutoMapperConfig();
                services.AddProblemDetailsConfig();
                services.AddTokenConfig();
                services.AddMediatR(typeof(GetUserCredentials).Assembly);
                services.AddScoped<IReportSender, ReportSender>();

                services.AddTransient<IEmailSender, EmailSender>();
                services.AddScoped<BuildingUpgradeSeed>();

                services.AddScoped<IIdentityContext, MockedIdentityContext>();


                var sp = services.BuildServiceProvider();

                var db = sp.GetRequiredService<ApplicationDbContext>();
                var buildingUpgradeSeed = sp.GetRequiredService<BuildingUpgradeSeed>();

                db.Database.Migrate();
                buildingUpgradeSeed.InitializeDbForTestsAsync(db).Wait();

                ServiceProvider = services.BuildServiceProvider();
            });
        }
    }
}