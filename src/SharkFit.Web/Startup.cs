using AspNetCore.ClaimsValueProvider;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;

using LiteDB;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharkFit.Data.Model;

using IdentityRole = AspNetCore.Identity.LiteDB.IdentityRole;

namespace SharkFit.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(service => new LiteDatabase("Filename=data/sharkfit.db;mode=Exclusive;"));
            services.AddSingleton(service => new LiteDbContext(service.GetService<IHostingEnvironment>())
            {
                LiteDatabase = service.GetService<LiteDatabase>()
            });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddUserStore<LiteDbUserStore<ApplicationUser>>()
                .AddRoleStore<LiteDbRoleStore<IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = _configuration["Authentication:Microsoft:ApplicationId"];
                options.ClientSecret = _configuration["Authentication:Microsoft:Password"];
            });

            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Challenge>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Checkin>());

            services.AddHttpsRedirection(options => options.HttpsPort = 443);

            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
            
            services.AddMvc(options => options.AddClaimsValueProvider());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
