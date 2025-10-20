using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }

        [Required] public int VersionId { get; set; }
        [Required] public int ColorId { get; set; }

        [Range(0, int.MaxValue)]
        public int? Quantity { get; set; }
    }
}
