using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Model
{
    public int ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Segment { get; set; }

    public virtual ICollection<Version> Versions { get; set; } = new List<Version>();
}
