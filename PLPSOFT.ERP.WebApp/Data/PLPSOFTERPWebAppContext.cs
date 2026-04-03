using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.WebApp.Data
{
    public class PLPSOFTERPWebAppContext : DbContext
    {
        public PLPSOFTERPWebAppContext (DbContextOptions<PLPSOFTERPWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<PLPSOFT.ERP.Domain.Entities.MasterData.CustomerGroupProductPrice> CustomerGroupProductPrice { get; set; } = default!;
        public DbSet<PLPSOFT.ERP.Domain.Entities.MasterData.ProductUnitMapping> ProductUnitMapping { get; set; } = default!;
    }
}
