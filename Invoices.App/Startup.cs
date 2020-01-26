using Hangfire;
using Hangfire.MemoryStorage;
using Invoices.App.Services.Email;
using Invoices.App.Services.Orders;
using Invoices.Data.Persistence;
using Invoices.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Invoices.App
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
			services.AddControllersWithViews();
			services.AddDbContext<InvoicesDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

			services.AddSingleton<InvoiceTracker>();
			services.AddScoped<OrderGrouping>();
			services.AddScoped<CreateInvoiceJob>();

			/*if (_environment.IsDevelopment())
			{
				services.AddHangfire(x => x.UseMemoryStorage());

				services.AddScoped<IUnitOfWork, InvoicesDbUnitOfWork>();
			}
			else
			{*/
				services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDatabaseConnection")));

				services.AddScoped<IUnitOfWork, InvoicesDbUnitOfWork>();
			//}
			services.AddHangfireServer();

			string emailServiceTypeName = Environment.GetEnvironmentVariable("EMAIL_SERVICE") + "EmailService";
			Type emailServiceType = Type.GetType("Invoices.App.Services.Email." + emailServiceTypeName);

			if (emailServiceType.Equals(typeof(SendGridEmailService)))
			{
				services.AddSingleton<IEmailService, SendGridEmailService>();
			} else
			{
				services.AddSingleton<IEmailService, FakeEmailService>();
			}
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
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseHangfireServer();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Invoices}/{action=Index}/{id?}");
			});
		}
	}
}
