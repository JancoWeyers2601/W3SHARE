using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using W3SHARE.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace W3SHARE.Data
{
    public partial class W3SHAREContext : DbContext
    {
        public W3SHAREContext()
        {
        }

        public W3SHAREContext(DbContextOptions<W3SHAREContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Access> Access { get; set; }
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<Metadata> Metadata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO: UPDATE CONNECTION STRING TO READ FROM appsettings.json
#warning        To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:w3share-prod.database.windows.net,1433;Initial Catalog=W3SHARE;Persist Security Info=False;User ID=w3share_admin;Password=9801265223083@Nwu;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Access>(entity =>
            {
                entity.Property(e => e.AccessId)
                    .HasColumnName("AccessID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.AlbumId)
                    .HasColumnName("AlbumID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileId)
                    .HasColumnName("FileID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AlbumId).HasColumnName("AlbumID");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.DateModified).HasColumnType("date");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Metadata>(entity =>
            {
                entity.Property(e => e.MetadataId)
                    .HasColumnName("Metadata_ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CaptureBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CaptureDate).HasColumnType("date");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.GeoLocation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tags)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
