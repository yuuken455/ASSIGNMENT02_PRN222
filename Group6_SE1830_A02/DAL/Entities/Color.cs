using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Color
{
    public int ColorId { get; set; }

    public int VersionId { get; set; }

    public string? ColorName { get; set; }

    public string? HexCode { get; set; }

    public decimal? ExtraCost { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<TestDriveAppointment> TestDriveAppointments { get; set; } = new List<TestDriveAppointment>();

    public virtual Version Version { get; set; } = null!;
}
