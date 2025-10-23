namespace BLL.DTOs
{
    public class StaffDto
    {
        public int StaffId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public int? DealerId { get; set; }
    }
}
