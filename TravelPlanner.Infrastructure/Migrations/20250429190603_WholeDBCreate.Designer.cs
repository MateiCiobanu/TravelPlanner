﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TravelPlanner.Infrastructure.Persistence;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250429190603_WholeDBCreate")]
    partial class WholeDBCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TravelPlanner.Domain.Entities.ItineraryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DayNumber")
                        .HasColumnType("integer");

                    b.Property<int?>("Duration")
                        .HasColumnType("integer");

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("interval");

                    b.Property<string>("GooglePlaceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaceCategory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("interval");

                    b.Property<int>("TripId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.ToTable("ItineraryItems");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.TravelerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PreferenceWeight")
                        .HasColumnType("integer");

                    b.Property<string>("TravelerTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TravelerTypes");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.ItineraryItem", b =>
                {
                    b.HasOne("TravelPlanner.Domain.Entities.Trip", "Trip")
                        .WithMany("ItineraryItems")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.TravelerType", b =>
                {
                    b.HasOne("TravelPlanner.Domain.Models.User", "User")
                        .WithMany("TravelerTypes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.Trip", b =>
                {
                    b.HasOne("TravelPlanner.Domain.Models.User", "User")
                        .WithMany("Trips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Entities.Trip", b =>
                {
                    b.Navigation("ItineraryItems");
                });

            modelBuilder.Entity("TravelPlanner.Domain.Models.User", b =>
                {
                    b.Navigation("TravelerTypes");

                    b.Navigation("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
