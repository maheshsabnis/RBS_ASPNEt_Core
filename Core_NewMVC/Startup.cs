using Core_NewMVC.Data;
using Core_NewMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_NewMVC.CustomFIlters;
namespace Core_NewMVC
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
			services.ConfigureApplicationCookie(options => {
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/Login";
				options.AccessDeniedPath = "/AccessDenied";
				options.SlidingExpiration = true;
				options.Events = new CookieAuthenticationEvents
				{
					OnRedirectToLogin = ctx => {
						var requestPath = ctx.Request.Path;
						if (requestPath.StartsWithSegments("/Account"))
						{
							ctx.Response.Redirect("/Account?ReturnUrl=" + requestPath + ctx.Request.QueryString);
						}
						else
						{
							ctx.Response.Redirect("/Login?ReturnUrl=" + requestPath + ctx.Request.QueryString);
						}
						return Task.CompletedTask;
					}
				};
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));


			services.AddDbContext<CompanyContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("AppDbConnection")));

			//services.AddDefaultIdentity<IdentityUser>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
			//	.AddEntityFrameworkStores<ApplicationDbContext>();

			// the Role BAsed Authentication

			services.AddIdentity<IdentityUser,IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
				.AddEntityFrameworkStores<ApplicationDbContext>();


			services.AddAuthentication();

			// defining Authorization Policies
			services.AddAuthorization(options=> {
				options.AddPolicy("ReadPolicy", policy=> {
					policy.RequireRole("Admin", "Manager", "Clerk");
				});
				options.AddPolicy("WritePolicy", policy => {
					policy.RequireRole("Admin", "Manager");
				});
			});

			// COnfguration of Session State Support for the Application
			services.AddDistributedMemoryCache();
			services.AddSession(session=> {
				session.IdleTimeout = TimeSpan.FromMinutes(20);
			});


			services.AddControllersWithViews(options=> {
				// options.Filters.Add(new LogActonFilterAttribute());
				options.Filters.Add(typeof(AppExceptionFilterAttribute));
			});
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			// Ask the HttpContext to use the Session Store to perform Session Read / Write Operations
			app.UseSession();


			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				
			});
		}
	}
}
