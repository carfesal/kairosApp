﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kairosApp.Domain.Persistence.Contexts;

#nullable disable

namespace kairosApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("kairosApp.Models.CuentaUsuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonaId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId")
                        .IsUnique();

                    b.ToTable("CuentaUsuarios");
                });

            modelBuilder.Entity("kairosApp.Models.Grupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Grupos");
                });

            modelBuilder.Entity("kairosApp.Models.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("CorreoAlterno")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Identificacion")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("Unidad")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("personas");
                });

            modelBuilder.Entity("kairosApp.Models.Solicitud", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("TEXT");

                    b.Property<string>("InfoSolicitud")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId");

                    b.ToTable("Solicitudes");
                });

            modelBuilder.Entity("kairosApp.Models.UsuarioGrupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CuentaUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GrupoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CuentaUsuarioId");

                    b.ToTable("UsuarioGrupo");
                });

            modelBuilder.Entity("kairosApp.Models.CuentaUsuario", b =>
                {
                    b.HasOne("kairosApp.Models.Persona", "Persona")
                        .WithOne("CuentaUsuario")
                        .HasForeignKey("kairosApp.Models.CuentaUsuario", "PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("kairosApp.Models.Solicitud", b =>
                {
                    b.HasOne("kairosApp.Models.Persona", "Persona")
                        .WithMany("Solicitudes")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("kairosApp.Models.UsuarioGrupo", b =>
                {
                    b.HasOne("kairosApp.Models.CuentaUsuario", "CuentaUsuario")
                        .WithMany("UsuarioGrupo")
                        .HasForeignKey("CuentaUsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kairosApp.Models.Grupo", "Grupo")
                        .WithMany("UsuarioGrupos")
                        .HasForeignKey("CuentaUsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CuentaUsuario");

                    b.Navigation("Grupo");
                });

            modelBuilder.Entity("kairosApp.Models.CuentaUsuario", b =>
                {
                    b.Navigation("UsuarioGrupo");
                });

            modelBuilder.Entity("kairosApp.Models.Grupo", b =>
                {
                    b.Navigation("UsuarioGrupos");
                });

            modelBuilder.Entity("kairosApp.Models.Persona", b =>
                {
                    b.Navigation("CuentaUsuario");

                    b.Navigation("Solicitudes");
                });
#pragma warning restore 612, 618
        }
    }
}
