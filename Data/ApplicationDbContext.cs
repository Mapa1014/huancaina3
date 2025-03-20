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

        
        protected override void OnModelCreating(ModelBuilder modelBuilder) //Confirguro nombres de las tablas
        {
            modelBuilder.Entity<Usuarios>()
                .HasKey(u => u.IdUsuario);

            modelBuilder.Entity<Ordenes>()
                .HasKey(o => o.IdOrden);

            modelBuilder.Entity<Inventarios>()
                .HasKey(i => i.IdInventario);

            modelBuilder.Entity<Productos>()
                .HasKey(p => p.IdProducto);   

            base.OnModelCreating(modelBuilder);
        }

    }
}
