using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediCase.WebAPI.Entities.Moderator;

public partial class MediCaseModeratorContext : DbContext
{
    public MediCaseModeratorContext()
    {
    }

    public MediCaseModeratorContext(DbContextOptions<MediCaseModeratorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EntitiesGraphDatum> EntitiesGraphData { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityLanguage> EntityLanguages { get; set; }

    public virtual DbSet<EntityTranslation> EntityTranslations { get; set; }

    public virtual DbSet<EntityTranslationFile> EntityTranslationFiles { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<ModeratorQueryBucket> ModeratorQueryBuckets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:Moderator", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.14-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<EntitiesGraphDatum>(entity =>
        {
            entity.HasKey(e => e.EdgeId).HasName("PRIMARY");

            entity
                .ToTable("entitiesgraphdata")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.ChildId, "ChildId");

            entity.HasIndex(e => new { e.ParentId, e.ChildId }, "ParentId").IsUnique();

            entity.Property(e => e.EdgeId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.ChildId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.ParentId).HasColumnType("int(10) unsigned");

            entity.HasOne(d => d.Child).WithMany(p => p.EntitiesGraphDatumChildren)
                .HasForeignKey(d => d.ChildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EntitiesGraphData_ibfk_2");

            entity.HasOne(d => d.Parent).WithMany(p => p.EntitiesGraphDatumParents)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("EntitiesGraphData_ibfk_1");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity
                .ToTable("entities")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.TypeId, "TypeId");

            entity.Property(e => e.EntityId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.EntityOrder).HasColumnType("bigint(20) unsigned");
            entity.Property(e => e.LockExpirationDate)
                .HasDefaultValueSql("utc_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnType("int(10) unsigned");

            entity.HasOne(d => d.Type).WithMany(p => p.Entities)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("Entities_ibfk_1");
        });

        modelBuilder.Entity<EntityLanguage>(entity =>
        {
            entity.HasKey(e => e.LangId).HasName("PRIMARY");

            entity
                .ToTable("entitylanguages")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.LangValue, "LangValue").IsUnique();

            entity.Property(e => e.LangId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.LangValue).HasMaxLength(10);
        });

        modelBuilder.Entity<EntityTranslation>(entity =>
        {
            entity.HasKey(e => e.TranslationId).HasName("PRIMARY");

            entity
                .ToTable("entitytranslations")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => new { e.EntityId, e.LangId }, "EntityId").IsUnique();

            entity.HasIndex(e => e.LangId, "LangId");

            entity.Property(e => e.TranslationId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.EntityId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.LangId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.MainTitle).HasColumnType("text");
            entity.Property(e => e.Paragrahps).HasColumnType("text");
            entity.Property(e => e.SubTitle).HasColumnType("text");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityTranslations)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("EntityTranslations_ibfk_2");

            entity.HasOne(d => d.Lang).WithMany(p => p.EntityTranslations)
                .HasForeignKey(d => d.LangId)
                .HasConstraintName("EntityTranslations_ibfk_1");
        });

        modelBuilder.Entity<EntityTranslationFile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PRIMARY");

            entity
                .ToTable("entitytranslationfiles")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.FilePath, "FilePath").IsUnique();

            entity.HasIndex(e => e.FilePathHashed, "FilePathHashed").IsUnique();

            entity.HasIndex(e => e.TranslationId, "TranslationId");

            entity.Property(e => e.FileId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.FilePathHashed).HasMaxLength(500);
            entity.Property(e => e.FilePriority).HasColumnType("int(10) unsigned");
            entity.Property(e => e.FileType).HasMaxLength(50);
            entity.Property(e => e.ReferredField).HasMaxLength(20);
            entity.Property(e => e.TranslationId).HasColumnType("int(10) unsigned");

            entity.HasOne(d => d.Translation).WithMany(p => p.EntityTranslationFiles)
                .HasForeignKey(d => d.TranslationId)
                .HasConstraintName("EntityTranslationFiles_ibfk_1");
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity
                .ToTable("entitytypes")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.TypeValue, "TypeValue").IsUnique();

            entity.Property(e => e.TypeId).HasColumnType("int(10) unsigned");
            entity.Property(e => e.TypeValue).HasMaxLength(50);
        });

        modelBuilder.Entity<ModeratorQueryBucket>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PRIMARY");

            entity
                .ToTable("moderatorquerybucket")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.OperationId).HasColumnType("bigint(20) unsigned");
            entity.Property(e => e.DestinationTable).HasMaxLength(50);
            entity.Property(e => e.OperationType).HasMaxLength(20);
            entity.Property(e => e.QueryLog).HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
