using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public class CustomerGroupProductPrice
    {
        [Key]
        public long GroupPriceId { get; set; }

        public long CustomerGroupId { get; set; }

        public long ProductId { get; set; }

        public decimal Price { get; set; } // Bỏ dấu ? nếu muốn bắt buộc nhập giá

        public decimal? DiscountRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }

        // QUAN TRỌNG: Thêm dòng này để thực hiện logic Xóa mềm của Leader
        public bool IsDelete { get; set; } = false;
    }
}