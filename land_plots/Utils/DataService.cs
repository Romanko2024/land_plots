using LandManagementApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LandManagementApp.Models.DTO;
using System.Windows;

namespace LandManagementApp.Utils
{
    //клас для збереження та завантаження об'єкта Settlement у файл
    public static class DataService
    {
        private const string FilePath = "data.json";

        public static void SaveData(Settlement settlement)
        {
            // Конвертація Settlement -> SettlementDTO
            var dto = ConvertToSettlementDTO(settlement);
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Settlement LoadData()
        {
            if (!File.Exists(FilePath)) return new Settlement();

            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<SettlementDTO>(json);

            // Конвертація SettlementDTO -> Settlement
            return ConvertFromSettlementDTO(dto);
        }

        private static SettlementDTO ConvertToSettlementDTO(Settlement settlement)
        {
            return new SettlementDTO
            {
                SerialNumber = settlement.SerialNumber,
                LandPlots = settlement.LandPlots.Select(ConvertToLandPlotDTO).ToList()
            };
        }

        private static LandPlotDTO ConvertToLandPlotDTO(LandPlot plot)
        {
            return new LandPlotDTO
            {
                Owner = new OwnerDTO
                {
                    FirstName = plot.Owner.FirstName,
                    LastName = plot.Owner.LastName,
                    BirthDate = plot.Owner.BirthDate
                },
                Description = new DescriptionDTO
                {
                    GroundWaterLevel = plot.Description.GroundWaterLevel,
                    Polygon = new List<Point>(plot.Description.Polygon)
                },
                Purpose = plot.Purpose,
                MarketValue = plot.MarketValue
            };
        }

        private static Settlement ConvertFromSettlementDTO(SettlementDTO dto)
        {
            var settlement = new Settlement();
            foreach (var plotDto in dto.LandPlots)
            {
                settlement.AddLandPlot(ConvertFromLandPlotDTO(plotDto));
            }
            return settlement;
        }

        private static LandPlot ConvertFromLandPlotDTO(LandPlotDTO dto)
        {
            var owner = new Owner(dto.Owner.FirstName, dto.Owner.LastName, dto.Owner.BirthDate);
            var description = new Description(dto.Description.GroundWaterLevel, dto.Description.Polygon);
            return new LandPlot(owner, description, dto.Purpose, dto.MarketValue);
        }
    }
}
