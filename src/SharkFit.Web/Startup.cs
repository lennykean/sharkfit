using AspNetCore.ClaimsValueProvider;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;

using LiteDB;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SharkFit.Data.Model;

using IdentityRole = AspNetCore.Identity.LiteDB.IdentityRole;

namespace SharkFit.Web
{
    public class Startup
    {
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

            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Challenge>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Checkin>());

            services.AddMvc(options => options.AddClaimsValueProvider());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
