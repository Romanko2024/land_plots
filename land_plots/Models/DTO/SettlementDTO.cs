﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandManagementApp.Models.DTO
{
    public class SettlementDTO
    {
        public int SerialNumber { get; set; }
        public string Name { get; set; }
        public List<LandPlotDTO> LandPlots { get; set; }
        public int NextSerialNumber { get; set; }
    }
}
