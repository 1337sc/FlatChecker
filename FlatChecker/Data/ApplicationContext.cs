#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlatChecker.Models;

namespace FlatChecker.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        public DbSet<Suggestion> Suggestions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<MinsFromPublicTransportMap> MinsFromPublicTransportMaps { get; set; }
    }
}
