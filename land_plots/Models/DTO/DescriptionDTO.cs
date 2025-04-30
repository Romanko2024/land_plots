using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LandManagementApp.Models.DTO
{
    public class DescriptionDTO
    {
        public int GroundWaterLevel { get; set; }
        public List<Point> Polygon { get; set; }
    }
}