﻿// <auto-generated />
using System;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SharedData.Models.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserIdentityFID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserIdentityFID");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("SharedData.Models.User.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ObservableFID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubscriberFID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ObservableFID");

                    b.HasIndex("SubscriberFID", "ObservableFID")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("SharedData.Models.User.UserIdentity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FaceId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UserIdentities");
                });

            modelBuilder.Entity("SharedData.Models.User.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserIdentityFID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserIdentityFID")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("SharedData.Models.Note", b =>
                {
                    b.HasOne("SharedData.Models.User.UserIdentity", "UserIdentity")
                        .WithMany("Notes")
                        .HasForeignKey("UserIdentityFID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("SharedData.Models.User.Subscription", b =>
                {
                    b.HasOne("SharedData.Models.User.UserIdentity", "Observable")
                        .WithMany("Observables")
                        .HasForeignKey("ObservableFID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SharedData.Models.User.UserIdentity", "Subscriber")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscriberFID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Observable");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("SharedData.Models.User.UserProfile", b =>
                {
                    b.HasOne("SharedData.Models.User.UserIdentity", "UserIdentity")
                        .WithOne("UserProfile")
                        .HasForeignKey("SharedData.Models.User.UserProfile", "UserIdentityFID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("SharedData.Models.User.UserIdentity", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("Observables");

                    b.Navigation("Subscribers");

                    b.Navigation("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
