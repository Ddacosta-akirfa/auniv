using Auniv.Models;
using Microsoft.EntityFrameworkCore;

namespace Auniv.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Universidade> Universidades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>()
            .HasOne(d => d.Universidade)
            .WithMany(u => u.Departamentos)
            .HasForeignKey(d => d.IdUniversidade);

        modelBuilder.Entity<Curso>()
            .HasOne(c => c.Departamento)
            .WithMany(d => d.Cursos)
            .HasForeignKey(c => c.IdDepartamento)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Universidade>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Universidade>()
            .Property(u => u.Tipo)
            .HasConversion<string>();

        modelBuilder.Entity<Universidade>()
            .OwnsOne(u => u.Localizacao, l =>
            {
                l.Property(loc => loc.Provincia)
                .HasConversion<string>();
            });

        modelBuilder.Entity<Curso>()
            .Property(c => c.Nivel)
            .HasConversion<string>();

        modelBuilder.Entity<Universidade>()
            .HasIndex(u => u.Sigla)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}