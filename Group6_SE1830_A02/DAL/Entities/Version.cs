using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Version
{
    public int VersionId { get; set; }

    public int ModelId { get; set; }

    public string VersionName { get; set; } = null!;

    public decimal? BatteryCapacity { get; set; }

    public int? RangeKm { get; set; }

    public int? Seat { get; set; }

    public decimal BasePrice { get; set; }

    public virtual ICollection<Color> Colors { get; set; } = new List<Color>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<TestDriveAppointment> TestDriveAppointments { get; set; } = new List<TestDriveAppointment>();
}
