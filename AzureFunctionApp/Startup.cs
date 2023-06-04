using System;
using AzureFunctionApp;
using AzureFunctionApp.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureFunctionApp
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureSqlConnection");

            builder.Services.AddDbContext<AzureDbContext>(
                options => options.UseSqlServer(connectionString));

            builder.Services.BuildServiceProvider();
        }
    }
}
