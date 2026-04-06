using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    [Table("SystemTypeValues", Schema = "core")] // 🔥 QUAN TRỌNG
    public class SystemTypeValue
    {
        [Key]
        public long TypeValueID { get; set; }

        public long TypeID { get; set; }
        public string ValueCode { get; set; }
        public string ValueName { get; set; }
        public bool IsActive { get; set; }
    }
}
