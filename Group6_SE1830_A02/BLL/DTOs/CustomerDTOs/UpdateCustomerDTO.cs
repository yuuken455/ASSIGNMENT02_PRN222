using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.CustomerDTOs
{
    public class UpdateCustomerDTO
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(150, ErrorMessage = "Họ và tên không được vượt quá 150 ký tự.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "CCCD/CMND là bắt buộc.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD/CMND phải có đúng 12 chữ số.")]
        public string Idnumber { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreateCustomerDTO), nameof(ValidateAge))]
        public DateOnly? Dob { get; set; }

        public string? Note { get; set; }

        public static ValidationResult? ValidateAge(DateOnly? dob, ValidationContext context)
        {
            if (dob == null)
                return new ValidationResult("Ngày sinh là bắt buộc.");

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dob.Value.Year;
            if (dob.Value > today.AddYears(-age)) age--;

            return age < 18
                ? new ValidationResult("Khách hàng phải đủ 18 tuổi trở lên.")
                : ValidationResult.Success;
        }
    }
}
