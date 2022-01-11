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

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

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

                    b.HasData(
                        new
                        {
                            Id = 101,
                            Alias = "carlos.salazar",
                            IsActive = true,
                            PersonaId = 100,
                            Username = "carfesal"
                        },
                        new
                        {
                            Id = 102,
                            Alias = "melanie.banchon",
                            IsActive = true,
                            PersonaId = 101,
                            Username = "meldaban"
                        },
                        new
                        {
                            Id = 103,
                            Alias = "melissa.pachar",
                            IsActive = true,
                            PersonaId = 102,
                            Username = "melroxan"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 101,
                            Nombre = "FIEC"
                        },
                        new
                        {
                            Id = 102,
                            Nombre = "FCSH"
                        },
                        new
                        {
                            Id = 103,
                            Nombre = "FCNM"
                        },
                        new
                        {
                            Id = 104,
                            Nombre = "FCV"
                        },
                        new
                        {
                            Id = 105,
                            Nombre = "FADCOM"
                        },
                        new
                        {
                            Id = 106,
                            Nombre = "FIMCP"
                        },
                        new
                        {
                            Id = 107,
                            Nombre = "FIMCBOR"
                        },
                        new
                        {
                            Id = 108,
                            Nombre = "Rectorado"
                        },
                        new
                        {
                            Id = 109,
                            Nombre = "Vinculos"
                        },
                        new
                        {
                            Id = 110,
                            Nombre = "STEM"
                        });
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

                    b.ToTable("Personas");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Apellidos = "Salazar Moreira",
                            CorreoAlterno = "chardan1225@hotmail.com",
                            Identificacion = "0940529829",
                            Nombres = "Carlos Fernando",
                            Rol = "Admin",
                            Telefono = "0990964027",
                            Unidad = "FIEC"
                        },
                        new
                        {
                            Id = 101,
                            Apellidos = "Banchon Chavez",
                            CorreoAlterno = "melanie1998@hotmail.com",
                            Identificacion = "0925153212",
                            Nombres = "Melanie Dayanna",
                            Rol = "Estudiante",
                            Telefono = "0987654321",
                            Unidad = "FIEC"
                        },
                        new
                        {
                            Id = 102,
                            Apellidos = "Pachar Duche",
                            CorreoAlterno = "melissa_pachar@hotmail.com",
                            Identificacion = "09876543231",
                            Nombres = "Melissa Roxanna",
                            Rol = "Estudiante",
                            Telefono = "0978791998",
                            Unidad = "Rectorado"
                        });
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

                    b.HasKey("Id");

                    b.ToTable("Solicitudes");

                    b.HasData(
                        new
                        {
                            Id = 101,
                            Estado = "No Revisado",
                            FechaCreacion = new DateTime(2022, 1, 11, 0, 28, 8, 804, DateTimeKind.Local).AddTicks(5660),
                            InfoSolicitud = "{\"nombres\":\"Fernando Carlos\",\"apellidos\":\"Moreira Salazar\",\"identificacion\":\"0912654798\",\"actividad\":\"Estudiante\",\"unidad\":\"FIEC\",\"telefono\":\"0997063143\",\"correo\":\"fernandomoreira@gmail.com\",\"alias_sugerido\":\"fernando.moreira\",\"usuario_sugerido\":\"fermorsa\"}"
                        },
                        new
                        {
                            Id = 102,
                            Estado = "No Revisado",
                            FechaCreacion = new DateTime(2022, 1, 11, 0, 28, 8, 804, DateTimeKind.Local).AddTicks(5823),
                            InfoSolicitud = "{\"nombres\":\"Rosa Cristina\",\"apellidos\":\"Alvarado Castillo\",\"identificacion\":\"0914365487\",\"actividad\":\"Estudiante\",\"unidad\":\"FIMCP\",\"telefono\":\"0991193877\",\"correo\":\"rosita_alvarado@gmail.com\",\"alias_sugerido\":\"rosa.cristina\",\"usuario_sugerido\":\"rosaalva\"}"
                        });
                });

            modelBuilder.Entity("kairosApp.Models.SolicitudPersona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PersonaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SolicitudId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId");

                    b.HasIndex("SolicitudId")
                        .IsUnique();

                    b.ToTable("SolicitudPersonas");
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

                    b.HasIndex("GrupoId");

                    b.ToTable("UsuarioGrupos");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            CuentaUsuarioId = 101,
                            GrupoId = 101
                        },
                        new
                        {
                            Id = 101,
                            CuentaUsuarioId = 102,
                            GrupoId = 103
                        },
                        new
                        {
                            Id = 102,
                            CuentaUsuarioId = 102,
                            GrupoId = 104
                        },
                        new
                        {
                            Id = 103,
                            CuentaUsuarioId = 103,
                            GrupoId = 101
                        },
                        new
                        {
                            Id = 104,
                            CuentaUsuarioId = 103,
                            GrupoId = 110
                        },
                        new
                        {
                            Id = 105,
                            CuentaUsuarioId = 102,
                            GrupoId = 109
                        });
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

            modelBuilder.Entity("kairosApp.Models.SolicitudPersona", b =>
                {
                    b.HasOne("kairosApp.Models.Persona", "Persona")
                        .WithMany("SolicitudPersonas")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kairosApp.Models.Solicitud", "Solicitud")
                        .WithOne("SolicitudPersona")
                        .HasForeignKey("kairosApp.Models.SolicitudPersona", "SolicitudId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");

                    b.Navigation("Solicitud");
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
                        .HasForeignKey("GrupoId")
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

                    b.Navigation("SolicitudPersonas");
                });

            modelBuilder.Entity("kairosApp.Models.Solicitud", b =>
                {
                    b.Navigation("SolicitudPersona")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
