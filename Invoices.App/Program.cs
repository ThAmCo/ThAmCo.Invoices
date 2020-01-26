using Invoices.Data.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Invoices.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var env = services.GetRequiredService<IWebHostEnvironment>();
				if (env.IsDevelopment())
				{
					var context = services.GetRequiredService<InvoicesDbContext>();
					//context.Database.EnsureDeleted();
					context.Database.Migrate();
					try
					{
						InvoicesDbInitialiser.SeedTestData(context, services).Wait();
					}
					catch (Exception e)
					{
						Console.WriteLine("Seeding test data failed. \n" + e.ToString());
					}
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
	}
}
