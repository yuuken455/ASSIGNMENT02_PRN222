using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class TestDriveAppointmentDto
    {
        public int AppointmentId { get; set; }

        [Display(Name = "Khách hàng")]
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Phiên bản xe")]
        [Required(ErrorMessage = "Vui lòng chọn phiên bản xe")]
        public int CarVersionId { get; set; }

        [Display(Name = "Màu xe")]
        [Required(ErrorMessage = "Vui lòng chọn màu xe")]
        public int ColorId { get; set; }

        [Display(Name = "Thời gian lái thử")]
        [Required(ErrorMessage = "Vui lòng chọn thời gian lái thử")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Trạng thái")]
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Phản hồi")]
        [StringLength(500)]
        public string? Feedback { get; set; }

        // ====== Thông tin hiển thị thêm ======
        public string? CustomerName { get; set; }
        public string? ModelName { get; set; }
        public int? ModelId { get; set; }
        public string? VersionName { get; set; }
        public string? ColorName { get; set; }
    }
}
