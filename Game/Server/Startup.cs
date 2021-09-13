using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Implementations;
using BackEnd.Repositories.Interfaces;
using Game.Server.Extensions;
using Hellang.Middleware.ProblemDetails;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services.Implementations;
using Services.Interfaces;
using Services.Queries;
using Hangfire;
using Hangfire.SqlServer;
using System;
using Hangfire.MediatR;
using Hangfire.Common;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Services.Commands.Game;

namespace Game.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Add Hangfire services.
            services.AddHangfire(configuration =>
            {
                configuration
                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseSqlServerStorage(Configuration["Hangfire"], new SqlServerStorageOptions
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
                     Configuration["DataConnectionString"], sqlOptions =>
                     {
                         sqlOptions.MigrationsAssembly("Game.Server");
                     }));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddScoped(sp => new AuthOptions
            {
                Audience = Configuration["AuthenticationSettings:Audience"],
                Issuer = Configuration["AuthenticationSettings:Issuer"],
                Key = Configuration["AuthenticationSettings:Key"],
                SenderAddress = Configuration["EmailConfig:SenderAddress"],
                SenderPassword = Configuration["EmailConfig:SenderPassword"],
                AppUrl = Configuration["AppUrl"]
            });

            services.AddIdentityConfig();
            services.AddIdentityContextConfig();
            services.AddAutoMapperConfig();
            services.AddProblemDetailsConfig();
            services.AddTokenConfig();
            services.AddMediatR(typeof(GetUserCredentials).Assembly);
            services.AddScoped<IReportSender, ReportSender>();



            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(sp =>
            {
                return new AuthMessageSenderOptions()
                {
                    SendGridKey = Configuration["SendGridKey"],
                    SendGridUser = Configuration["SendGridUser"]
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "StrategyGame Api",
                        Description = "StrategyGame is a project made for my independent laboratory work",
                        Version = "v1.0"
                    });
                options.EnableAnnotations();
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin"));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IUnitOfWork unitOfWork, IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IMediator mediatr, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StrategyGame Web API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHangfireDashboard();

            var manager = new RecurringJobManager();
            manager.AddOrUpdate("IncreaseResources", Job.FromExpression(() => new ProduceResources(unitOfWork).Produce()), Cron.Hourly());
            manager.AddOrUpdate("IncreaseLoyalty", Job.FromExpression(() => new IncreaseLoyalty(unitOfWork).IncreaseCityLoyalty()), Cron.Hourly());


            app.UseBlazorFrameworkFiles();
            app.UseIdentityServer();
            app.UseProblemDetails();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
