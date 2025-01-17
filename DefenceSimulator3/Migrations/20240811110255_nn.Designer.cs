﻿// <auto-generated />
using System;
using DefenceSimulator3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DefenceSimulator3.Migrations
{
    [DbContext(typeof(DefenceSimulator3Context))]
    [Migration("20240811110255_nn")]
    partial class nn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DefenceSimulator3.Models.Origin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Origin");
                });

            modelBuilder.Entity("DefenceSimulator3.Models.Threat", b =>
                {
                    b.Property<int>("ThreatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThreatId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("Fail")
                        .HasColumnType("int");

                    b.Property<DateTime>("LaunchTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OriginId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("Success")
                        .HasColumnType("int");

                    b.Property<int>("WeaponId")
                        .HasColumnType("int");

                    b.HasKey("ThreatId");

                    b.HasIndex("OriginId");

                    b.HasIndex("WeaponId");

                    b.ToTable("Threat");
                });

            modelBuilder.Entity("DefenceSimulator3.Models.Weapon", b =>
                {
                    b.Property<int>("WeaponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WeaponId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<int>("WeaponDefenceId")
                        .HasColumnType("int");

                    b.HasKey("WeaponId");

                    b.HasIndex("WeaponDefenceId");

                    b.ToTable("Weapon");
                });

            modelBuilder.Entity("DefenceSimulator3.Models.WeaponDefence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeaponDefence");
                });

            modelBuilder.Entity("DefenceSimulator3.Models.Threat", b =>
                {
                    b.HasOne("DefenceSimulator3.Models.Origin", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DefenceSimulator3.Models.Weapon", "Weapon")
                        .WithMany()
                        .HasForeignKey("WeaponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Origin");

                    b.Navigation("Weapon");
                });

            modelBuilder.Entity("DefenceSimulator3.Models.Weapon", b =>
                {
                    b.HasOne("DefenceSimulator3.Models.WeaponDefence", "WeaponDefence")
                        .WithMany()
                        .HasForeignKey("WeaponDefenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeaponDefence");
                });
#pragma warning restore 612, 618
        }
    }
}
