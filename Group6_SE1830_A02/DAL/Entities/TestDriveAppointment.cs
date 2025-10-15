using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class TestDriveAppointment
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int CarVersionId { get; set; }

    public int ColorId { get; set; }

    public DateTime DateTime { get; set; }

    public string Status { get; set; } = null!;

    public string? Feedback { get; set; }

    public virtual Version CarVersion { get; set; } = null!;

    public virtual Color Color { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
