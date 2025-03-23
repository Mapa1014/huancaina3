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
        public DbSet<Usuarios> Usuarios { get; set; } //Definir las tablas que tendra la DB
        public DbSet<Ordenes> Ordenes { get; set; }
        public DbSet<Inventarios> Inventarios { get; set; }
        public DbSet<Productos> Productos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
                .HasKey(u => u.IdUsuario); // This is fine for the key configuration.

            modelBuilder.Entity<Usuarios>()
                .Property(u => u.IdUsuario)
                .HasColumnName("id_usuario"); // Map to the database column name.

            base.OnModelCreating(modelBuilder);
        }


    }
}
