using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebRestApp.Models.Db;

public partial class MedicineDbContext : DbContext
{
    public MedicineDbContext()
    {
    }

    public MedicineDbContext(DbContextOptions<MedicineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MedicineCategory> MedicineCategories { get; set; }

    public virtual DbSet<MedicineInventory> MedicineInventories { get; set; }

    public virtual DbSet<OperatorDetail> OperatorDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=MedicineDb;Integrated Security=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicineCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Medicine__19093A0B0DEAC5A9");

            entity.ToTable("MedicineCategory");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<MedicineInventory>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__4F212890E2EDA61C");

            entity.ToTable("MedicineInventory");

            entity.Property(e => e.Expirydate).HasColumnType("date");
            entity.Property(e => e.MedicineName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Category).WithMany(p => p.MedicineInventories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk");
        });

        modelBuilder.Entity<OperatorDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operator__3214EC07F625F4FF");

            entity.Property(e => e.DoB).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(64);
            entity.Property(e => e.Password).HasMaxLength(15);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC077BD60B5D");

            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
