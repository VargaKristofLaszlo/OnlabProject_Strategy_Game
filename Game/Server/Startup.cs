using BackEnd.Infrastructure;
using BackEnd.Models.DataSeeding;
using BackEnd.Models.Models;
using BackEnd.Repositories.Implementations;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                     {
                         sqlOptions.MigrationsAssembly("Game.Server");
                     }));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();


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
                      ValidAudience = Configuration["AuthenticationSettings:Audience"],
                      ValidIssuer = Configuration["AuthenticationSettings:Issuer"],
                      RequireExpirationTime = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationSettings:Key"])),
                      ValidateIssuerSigningKey = true
                  };
              });

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

            services.AddScoped(sp =>
            {
                var httpContext = sp.GetService<IHttpContextAccessor>().HttpContext;

                var identityOptions = new BackEnd.Infrastructure.IdentityOptions();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    string id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    string username = httpContext.User.FindFirst(ClaimTypes.Name).Value;
                    string email = httpContext.User.FindFirst(ClaimTypes.Email).Value;
                    bool isAdmin = "Admin".Equals(httpContext.User.FindFirst(ClaimTypes.Role).Value);

                    identityOptions.UserId = id;
                    identityOptions.Username = username;
                    identityOptions.Email = email;
                    identityOptions.IsAdmin = isAdmin;
                }
                return identityOptions;
            });

            services.AddHttpContextAccessor();
            services.AddScoped<BackEnd.Services.Interfaces.IAuthenticationService, BackEnd.Services.Implementations.AuthenticationService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IViewService, ViewService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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

            var dataSeeding = new UserSeeding(userManager, roleManager);
            dataSeeding.SeedData().Wait();
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
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
