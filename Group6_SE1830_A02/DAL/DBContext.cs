using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<TestDriveAppointment> TestDriveAppointments { get; set; }

    public virtual DbSet<Entities.Version> Versions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7676DC531BF66");

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(100);
            entity.Property(e => e.ExtraCost)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HexCode)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.VersionId).HasColumnName("VersionID");

            entity.HasOne(d => d.Version).WithMany(p => p.Colors)
                .HasForeignKey(d => d.VersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Colors__VersionI__403A8C7D");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B813CC8C78");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Idnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDNumber");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6D30DFF5557");

            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("InventoryID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.Quantity).HasDefaultValue(0);
            entity.Property(e => e.VersionId).HasColumnName("VersionID");

            entity.HasOne(d => d.Color).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Color__44FF419A");

            entity.HasOne(d => d.Version).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.VersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Versi__440B1D61");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK__Models__E8D7A1CC52969A9E");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModelName).HasMaxLength(150);
            entity.Property(e => e.Segment).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFFD0A7212");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__4F7CD00D");

            entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__StaffID__5070F446");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C941A3728");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VersionId).HasColumnName("VersionID");

            entity.HasOne(d => d.Color).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Color__5629CD9C");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__5441852A");

            entity.HasOne(d => d.Version).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.VersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Versi__5535A963");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AAF75C4A5752");

            entity.HasIndex(e => e.Email, "UQ__Staffs__A9D10534ABF062E9").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.DealerId).HasColumnName("DealerID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestDriveAppointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__TestDriv__8ECDFCA27F705F61");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CarVersionId).HasColumnName("CarVersionID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Feedback).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.CarVersion).WithMany(p => p.TestDriveAppointments)
                .HasForeignKey(d => d.CarVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestDrive__CarVe__4AB81AF0");

            entity.HasOne(d => d.Color).WithMany(p => p.TestDriveAppointments)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestDrive__Color__4BAC3F29");

            entity.HasOne(d => d.Customer).WithMany(p => p.TestDriveAppointments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestDrive__Custo__49C3F6B7");
        });

        modelBuilder.Entity<Entities.Version>(entity =>
        {
            entity.HasKey(e => e.VersionId).HasName("PK__Versions__16C6402F823BF93C");

            entity.Property(e => e.VersionId).HasColumnName("VersionID");
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BatteryCapacity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.VersionName).HasMaxLength(150);

            entity.HasOne(d => d.Model).WithMany(p => p.Versions)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Versions__ModelI__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
