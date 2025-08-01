using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PrivilegeAPI.Models;

namespace PrivilegeAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
