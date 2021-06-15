using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snackis.Data;
using Snackis.Data.Models;
using Snackis.Gateway;
using Snackis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RoleMustBeAdmin",
                    policy => policy.RequireRole("Admin"));
                options.AddPolicy("RoleMustBeUser",
                    policy => policy.RequireRole("User"));
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);    //NYTT KANSKE SKA VARA TRUE??

            services.AddHttpClient<BadWordGateway>();
            services.AddTransient<IBadWordGateway, BadWordGateway>();
            services.AddTransient<IGroupServices, GroupServices>();
            services.AddTransient<IAdminServices, AdminServices>();
            services.AddTransient<IMessageServices, MessageServices>();
            services.AddTransient<ISubThreadServices, SubThreadServices>();
            services.AddTransient<IMainThreadServices, MainThreadServices>();
            services.AddTransient<IUserServices, UserServices>();

            services.AddDbContext<SnackisContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SnackisContextConnection")));

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin", "RoleMustBeAdmin");
                options.Conventions.AuthorizePage("/UserInfo", "RoleMustBeUser");
                options.Conventions.AuthorizePage("/GroupMessages"); 
                options.Conventions.AuthorizePage("/NewSubThread");
                options.Conventions.AuthorizePage("/ReportMessagePage");
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
