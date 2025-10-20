using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ModelDto
    {
        public int ModelId { get; set; }
        [Required, StringLength(100)]
        public string ModelName { get; set; } = null!;
        [StringLength(2000)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Segment { get; set; }
    }
}
