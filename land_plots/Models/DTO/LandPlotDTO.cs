using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandManagementApp.Models.DTO
{
    public class LandPlotDTO
    {
        public OwnerDTO Owner { get; set; }
        public DescriptionDTO Description { get; set; }
        public PurposeType Purpose { get; set; }
        public decimal MarketValue { get; set; }
    }
}
