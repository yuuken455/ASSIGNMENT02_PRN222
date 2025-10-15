using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int VersionId { get; set; }

    public int ColorId { get; set; }

    public int? Quantity { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Version Version { get; set; } = null!;
}
