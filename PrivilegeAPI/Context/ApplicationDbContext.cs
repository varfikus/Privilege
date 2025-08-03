using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PrivilegeAPI.Models;

namespace PrivilegeAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Models.File> Files { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasOne(a => a.File)
                .WithOne(f => f.Application)
                .HasForeignKey<Application>(a => a.FileId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
