using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Forum_rufine_et_paul.Models;

public partial class Forum23105Context : DbContext
{
    public Forum23105Context()
    {
    }

    public Forum23105Context(DbContextOptions<Forum23105Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatPk);

            entity.Property(e => e.CatPk).HasColumnName("Cat_Pk");
            entity.Property(e => e.CatActif)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CatDesc).HasMaxLength(500);
            entity.Property(e => e.CatImg).HasMaxLength(255);
            entity.Property(e => e.CatName).HasMaxLength(25);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestPk);

            entity.Property(e => e.QuestPk).HasColumnName("Quest_Pk");
            entity.Property(e => e.CatFk).HasColumnName("Cat_Fk");
            entity.Property(e => e.QuestActif)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.QuestDate).HasColumnType("datetime");
            entity.Property(e => e.QuestText).HasMaxLength(2000);
            entity.Property(e => e.QuestTitle).HasMaxLength(255);
            entity.Property(e => e.QuestViews).HasDefaultValueSql("((0))");
            entity.Property(e => e.UserFk)
                .HasMaxLength(450)
                .HasColumnName("User_Fk");

            entity.HasOne(d => d.CatFkNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CatFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_Categories");

            entity.HasOne(d => d.UserFkNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.UserFk)
                .HasConstraintName("FK_Questions_AspNetUsers");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.RespPk);

            entity.Property(e => e.RespPk).HasColumnName("Resp_Pk");
            entity.Property(e => e.QuestFk).HasColumnName("Quest_Fk");
            entity.Property(e => e.RespActif)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.RespDate).HasColumnType("datetime");
            entity.Property(e => e.RespText).HasMaxLength(4000);
            entity.Property(e => e.UserFk)
                .HasMaxLength(450)
                .HasColumnName("User_Fk");

            entity.HasOne(d => d.QuestFkNavigation).WithMany(p => p.Responses)
                .HasForeignKey(d => d.QuestFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Responses_Questions");

            entity.HasOne(d => d.UserFkNavigation).WithMany(p => p.Responses)
                .HasForeignKey(d => d.UserFk)
                .HasConstraintName("FK_Responses_AspNetUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
