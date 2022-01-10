using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace kairosApp.Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Persona> Personas { get; set; }
        public DbSet<CuentaUsuario> CuentaUsuarios { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<UsuarioGrupo> UsuarioGrupos { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //PERSONA
            builder.Entity<Persona>().HasKey(p => p.Id);
            builder.Entity<Persona>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Persona>().Property(p => p.Nombres).IsRequired().HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Apellidos).IsRequired().HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Telefono).HasMaxLength(10);
            builder.Entity<Persona>().Property(p => p.Identificacion).IsRequired().HasMaxLength(10);
            builder.Entity<Persona>().Property(p => p.Unidad).HasMaxLength(20);
            builder.Entity<Persona>().Property(p => p.CorreoAlterno).HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Rol).HasMaxLength(20);
            builder.Entity<Persona>().HasOne(p => p.CuentaUsuario).WithOne(p => p.Persona).HasForeignKey<CuentaUsuario>(p => p.PersonaId);
            builder.Entity<Persona>().HasMany(p => p.Solicitudes).WithOne(p => p.Persona).HasForeignKey(p => p.PersonaId);

            /*builder.Entity<Category>().HasData
            (
                new Category { Id = 100, Name = "Fruits and Vegetables" }, // Id set manually due to in-memory provider
                new Category { Id = 101, Name = "Dairy" });*/
            //CUENTA USUARIO
            builder.Entity<CuentaUsuario>().HasKey(p => p.Id);
            builder.Entity<CuentaUsuario>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<CuentaUsuario>().Property(p => p.Username).IsRequired().HasMaxLength(10);
            builder.Entity<CuentaUsuario>().Property(p => p.Alias).IsRequired().HasMaxLength(20);
            builder.Entity<CuentaUsuario>().HasMany(p => p.UsuarioGrupo).WithOne(p => p.CuentaUsuario).HasForeignKey(p => p.CuentaUsuarioId);

            //GRUPO
            builder.Entity<Grupo>().HasKey(p => p.Id);
            builder.Entity<Grupo>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Grupo>().Property(p => p.Nombre).IsRequired().HasMaxLength(20);
            builder.Entity<Grupo>().HasMany(p => p.UsuarioGrupos).WithOne(p => p.Grupo).HasForeignKey(p => p.GrupoId);

            //SOLICITUD
            builder.Entity<Solicitud>().HasKey(p => p.Id);
            builder.Entity<Solicitud>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Solicitud>().Property(p => p.Estado).IsRequired().HasMaxLength(20);
            builder.Entity<Solicitud>().Property(p => p.FechaCreacion).IsRequired();
            builder.Entity<Solicitud>().Property(p => p.InfoSolicitud).IsRequired()
                .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<string, object>>(v));
            
            //UsuarioGrupo
            builder.Entity<UsuarioGrupo>().HasKey(p => p.Id);
            builder.Entity<UsuarioGrupo>().HasOne(p => p.CuentaUsuario).WithMany(p => p.UsuarioGrupo).HasForeignKey(p => p.CuentaUsuarioId);
            builder.Entity<UsuarioGrupo>().HasOne(p => p.Grupo).WithMany(p => p.UsuarioGrupos).HasForeignKey(p => p.CuentaUsuarioId);


        }
    }
}

