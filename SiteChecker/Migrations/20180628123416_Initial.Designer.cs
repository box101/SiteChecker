﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SiteChecker;

namespace SiteChecker.Migrations
{
    [DbContext(typeof(SiteCheckerDbContext))]
    [Migration("20180628123416_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SiteChecker.UrlCheckTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HttpStatusCode");

                    b.Property<DateTime?>("LastCheckDateTime");

                    b.Property<string>("LastCheckResultJson");

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Url");

                    b.ToTable("UrlCheckTasks");
                });

            modelBuilder.Entity("SiteChecker.UrlCheckTaskResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CheckDateTime");

                    b.Property<string>("LastCheckResultJson");

                    b.Property<int>("UrlCheckTaskId");

                    b.HasKey("Id");

                    b.HasIndex("UrlCheckTaskId");

                    b.ToTable("UrlCheckTaskResults");
                });

            modelBuilder.Entity("SiteChecker.UrlCheckTaskResult", b =>
                {
                    b.HasOne("SiteChecker.UrlCheckTask", "UrlCheckTask")
                        .WithMany()
                        .HasForeignKey("UrlCheckTaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
