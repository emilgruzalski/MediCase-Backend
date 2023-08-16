using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediCase.WebAPI.Entities.Admin;

public partial class MediCaseAdminContext : DbContext
{
    public MediCaseAdminContext()
    {
    }

    public MediCaseAdminContext(DbContextOptions<MediCaseAdminContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:Admin", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.14-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("group");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11) unsigned");
            entity.Property(e => e.Description).HasMaxLength(140);
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11) unsigned");
            entity.Property(e => e.Email).HasMaxLength(40);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(40);
            entity.Property(e => e.PasswordHash).HasMaxLength(60);

            entity.HasMany(d => d.Groups).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Usergroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_usergroup_group"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_usergroup_user"),
                    j =>
                    {
                        j.HasKey("UserId", "GroupId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("usergroup");
                        j.HasIndex(new[] { "GroupId" }, "FK_usergroup_group");
                        j.IndexerProperty<uint>("UserId").HasColumnType("int(11) unsigned");
                        j.IndexerProperty<uint>("GroupId").HasColumnType("int(11) unsigned");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
