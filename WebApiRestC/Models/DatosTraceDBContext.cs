using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApiRestC.Models
{
    public partial class DatosTraceDBContext : DbContext
    {
        public DatosTraceDBContext()
        {
        }

        public DatosTraceDBContext(DbContextOptions<DatosTraceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Eventos> Eventos { get; set; }
        public virtual DbSet<TodoItem> TodoItem { get; set; }
        //public object ClaseEvento { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //    //optionsBuilder.UseSqlServer("Server=WIN-3M01CWMY9N\\AACRSQLEXPRESS;Database=DatosTraceDB;User ID=MaryCarmen;Password=Consuelo0414");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eventos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsError)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Evento)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechayHora).HasColumnType("datetime");
            });

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
