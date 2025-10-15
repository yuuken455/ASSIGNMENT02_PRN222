using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int VersionId { get; set; }

    public int ColorId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Discount { get; set; }

    public decimal FinalPrice { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Version Version { get; set; } = null!;
}
