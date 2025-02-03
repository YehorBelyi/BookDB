using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Models;

public partial class BooksContext : DbContext
{
    public BooksContext()
    {
    }

    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksNew> BooksNews { get; set; }

    public virtual DbSet<SprFormat> SprFormats { get; set; }

    public virtual DbSet<SprIzd> SprIzds { get; set; }

    public virtual DbSet<SprKategory> SprKategories { get; set; }

    public virtual DbSet<SprTheme> SprThemes { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KOMPUTER\\SQLEXPRESS;Database=books;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.N);

            entity.ToTable("books");

            entity.Property(e => e.N).ValueGeneratedNever();
            entity.Property(e => e.Category).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Format).HasMaxLength(255);
            entity.Property(e => e.Izd).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Themes).HasMaxLength(255);
        });

        modelBuilder.Entity<BooksNew>(entity =>
        {
            entity.HasKey(e => e.N);

            entity.ToTable("books_new");

            entity.Property(e => e.N).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FormatId).HasColumnName("format_id");
            entity.Property(e => e.IzdId).HasColumnName("izd_id");
            entity.Property(e => e.KategoryId).HasColumnName("kategory_id");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ThemesId).HasColumnName("themes_id");

            entity.HasOne(d => d.Format).WithMany(p => p.BooksNews)
                .HasForeignKey(d => d.FormatId)
                .HasConstraintName("FK_books_new_Spr_format");

            entity.HasOne(d => d.Izd).WithMany(p => p.BooksNews)
                .HasForeignKey(d => d.IzdId)
                .HasConstraintName("FK_books_new_Spr_izd");

            entity.HasOne(d => d.Kategory).WithMany(p => p.BooksNews)
                .HasForeignKey(d => d.KategoryId)
                .HasConstraintName("FK_books_new_Spr_kategory");

            entity.HasOne(d => d.Themes).WithMany(p => p.BooksNews)
                .HasForeignKey(d => d.ThemesId)
                .HasConstraintName("FK_books_new_Spr_themes");
        });

        modelBuilder.Entity<SprFormat>(entity =>
        {
            entity.ToTable("Spr_format");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Format).HasMaxLength(255);
        });

        modelBuilder.Entity<SprIzd>(entity =>
        {
            entity.ToTable("Spr_izd");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Izd).HasMaxLength(255);
        });

        modelBuilder.Entity<SprKategory>(entity =>
        {
            entity.ToTable("Spr_kategory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Category).HasMaxLength(255);
        });

        modelBuilder.Entity<SprTheme>(entity =>
        {
            entity.ToTable("Spr_themes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Themes).HasMaxLength(255);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasMany(d => d.Books).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCard",
                    r => r.HasOne<BooksNew>().WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StudentCards_books_new"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StudentCards_Students"),
                    j =>
                    {
                        j.HasKey("StudentId", "BooksId").HasName("PK_StudentCards_1");
                        j.ToTable("StudentCards");
                        j.IndexerProperty<int>("StudentId").HasColumnName("Student_id");
                        j.IndexerProperty<int>("BooksId").HasColumnName("Books_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
