using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class VersionDto
    {
        public int VersionId { get; set; }
        [Required] public int ModelId { get; set; }
        [Required, StringLength(100)] public string VersionName { get; set; } = null!;
        public decimal? BatteryCapacity { get; set; }
        public int? RangeKm { get; set; }
        public int? Seat { get; set; }
        [Range(0, double.MaxValue)] public decimal BasePrice { get; set; }
    }
}
