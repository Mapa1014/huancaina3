using Microsoft.EntityFrameworkCore;
using huancaina.Models;
using System.Collections.Generic;

namespace huancaina.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
                .HasKey(u => u.IdUsuario);

            base.OnModelCreating(modelBuilder);
        }


    }
}
