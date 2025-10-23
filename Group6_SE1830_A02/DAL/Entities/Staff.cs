namespace DAL.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public int? DealerId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
