using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpperToolsProject.Models;

namespace UpperToolsProject.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Qsa> Qsa { get; set; }

    }
}
