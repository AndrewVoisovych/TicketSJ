using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TicketSJWindowsService.Models
{
    public partial class TicketSoftjournContext : DbContext
    {
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketTags> TicketTags { get; set; }
        public virtual DbSet<TicketToTags> TicketToTags { get; set; }
        public virtual DbSet<TicketType> TicketType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:sjtickets.database.windows.net,1433;Initial Catalog=TicketSoftjourn;Persist Security Info=False;User ID=vois;Password=PaSSword12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasIndex(e => e.Number)
                    .HasName("UQ__Ticket__78A1A19D460CC718")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDateTime).HasColumnType("datetime");

                entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_TicketType");
            });

            modelBuilder.Entity<TicketTags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.Property(e => e.TagId).ValueGeneratedNever();

                entity.Property(e => e.TagTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TicketToTags>(entity =>
            {
                entity.HasKey(e => new { e.TicketId, e.TagId });

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TicketToTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketToTags_TicketTags");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketToTags)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketToTags_Ticket");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
