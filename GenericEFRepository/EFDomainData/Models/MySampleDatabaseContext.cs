using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDomainData.Models
{
    public partial class MySampleDatabaseContext : DbContext
    {
        public MySampleDatabaseContext()
        {
        }
        public MySampleDatabaseContext(DbContextOptions<MySampleDatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<SampleTable> SampleTable { get; set; } = null!;
        public virtual DbSet<SampleTableDetail> SampleTableDetails { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=.;Database=MySampleDatabase;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("Server=.;Database=MySampleDatabase;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleTable>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(50);
            });
            modelBuilder.Entity<SampleTableDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
