using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureFunctionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctionApp.Data
{
    public class AzureDbContext : DbContext
    {
        public AzureDbContext(DbContextOptions<AzureDbContext> options) : base(options)
        {
        }
        public DbSet<SalesRequestModel> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesRequestModel>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
        }
    }
}
