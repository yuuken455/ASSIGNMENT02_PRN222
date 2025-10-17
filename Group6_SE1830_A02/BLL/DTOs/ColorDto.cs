using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ColorDto
    {
        public int ColorId { get; set; }
        [Required] public int VersionId { get; set; }
        [Required, StringLength(100)] public string ColorName { get; set; } = null!;
        [StringLength(7)] public string? HexCode { get; set; } // #RRGGBB
        [Range(0, double.MaxValue)] public decimal? ExtraCost { get; set; }
    }
}
