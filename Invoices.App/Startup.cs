using Hangfire;
using Hangfire.MemoryStorage;
using Invoices.App.Services.Email;
using Invoices.App.Services.Orders;
using Invoices.Data.Persistence;
using Invoices.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Invoices.App
{
	public class Startup
	{

		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			_environment = environment;
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

			if (_environment.IsDevelopment())
			{
				services.AddHangfire(x => x.UseMemoryStorage());

				services.AddScoped<IUnitOfWork, InvoicesDbUnitOfWork>();
			}
			else
			{
				services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDatabaseConnection")));

				services.AddScoped<IUnitOfWork, InvoicesDbUnitOfWork>();
			}
			services.AddHangfireServer();

			var emailServiceTypeName = Configuration["EMAIL_SERVICE"] + "EmailService";

			if (emailServiceTypeName.Equals(nameof(SendGridEmailService)))
			{
				services.AddSingleton<IEmailService, SendGridEmailService>();
			} else
			{
				services.AddSingleton<IEmailService, FakeEmailService>();
			}

			if (_environment.IsDevelopment())
			{
				services.AddDataProtection()
					.PersistKeysToFileSystem(new DirectoryInfo(@"c:/shared-cookies"))
					.SetApplicationName("ThAmCo");
			}
			else
			{
				var storageKey = Configuration["BLOB_STORAGE_KEY"];

				var credentials = new StorageCredentials("thamcostorage", storageKey);
				var storageAccount = new CloudStorageAccount(credentials, true);
				var blobClient = storageAccount.CreateCloudBlobClient();

				CloudBlobContainer container = blobClient.GetContainerReference("keys");

				services.AddDataProtection()
					.PersistKeysToAzureBlobStorage(container, "cookies")
					.SetApplicationName("ThAmCo");
			}

			var homeBaseUrl = Configuration["HOME_URL"];
			var authAuthorityUrl = Configuration["AUTHENTICATION_AUTHORITY"];

			services.AddAuthentication("Cookies")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = authAuthorityUrl;
					options.Audience = "thamco_invoices_api";
				})
				.AddCookie("Cookies", options =>
				{
					options.Cookie.Name = ".ThAmCo.SharedCookie";
					options.Cookie.Path = "/";
					options.LoginPath = "/Account/Login";
					options.LogoutPath = "/Account/Logout";

					options.Events.OnRedirectToLogin = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + options.LoginPath);
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToLogout = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + options.LogoutPath);
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToAccessDenied = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + "/accessdenied");
						return Task.CompletedTask;
					};
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("StaffOnly", builder =>
				{
					builder.RequireClaim("role", "Staff");
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseHangfireServer();

			app.UseAuthentication();
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
