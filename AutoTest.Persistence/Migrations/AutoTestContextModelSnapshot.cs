﻿// <auto-generated />
using System;
using AutoTest.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutoTest.Persistence.Migrations
{
    [DbContext(typeof(AutoTestContext))]
    partial class AutoTestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("AutoTest.Domain.StorageModels.AdminEmail", b =>
                {
                    b.Property<ulong>("AdminEmailId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ClubId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AdminEmailId");

                    b.HasIndex("ClubId");

                    b.ToTable("AdminEmail");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Club", b =>
                {
                    b.Property<ulong>("ClubId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClubName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClubPaymentAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ClubId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Entrant", b =>
                {
                    b.Property<ulong>("EntrantId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Registration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntrantId");

                    b.HasIndex("EventId");

                    b.ToTable("Entrant");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Event", b =>
                {
                    b.Property<ulong>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ClubId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.HasIndex("ClubId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Test", b =>
                {
                    b.Property<ulong>("TestId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MapLocation")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Ordinal")
                        .HasColumnType("INTEGER");

                    b.HasKey("TestId");

                    b.HasIndex("EventId");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.TestRun", b =>
                {
                    b.Property<ulong>("TestRunId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Entrant")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("TestId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("TimeInMS")
                        .HasColumnType("INTEGER");

                    b.HasKey("TestRunId");

                    b.HasIndex("Entrant");

                    b.HasIndex("TestId");

                    b.ToTable("TestRun");
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.AdminEmail", b =>
                {
                    b.HasOne("AutoTest.Domain.StorageModels.Club", null)
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Entrant", b =>
                {
                    b.HasOne("AutoTest.Domain.StorageModels.Event", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Event", b =>
                {
                    b.HasOne("AutoTest.Domain.StorageModels.Club", null)
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.Test", b =>
                {
                    b.HasOne("AutoTest.Domain.StorageModels.Event", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutoTest.Domain.StorageModels.TestRun", b =>
                {
                    b.HasOne("AutoTest.Domain.StorageModels.Entrant", null)
                        .WithMany()
                        .HasForeignKey("Entrant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoTest.Domain.StorageModels.Test", null)
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
