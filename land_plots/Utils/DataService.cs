using LandManagementApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using LandManagementApp.Models.DTO;

namespace LandManagementApp.Utils
{
    //клас для збереження та завантаження об'єкта Settlement у файл
    public static class DataService
    {
        private const string FilePath = "data.json";
        private static JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            Converters = { new PointConverter() },
            Formatting = Formatting.Indented
        };

        public static void SaveData(Settlement settlement)
        {
            // Конвертація Settlement -> SettlementDTO
            var dto = ConvertToSettlementDTO(settlement);
            var json = JsonConvert.SerializeObject(dto, _settings);
            File.WriteAllText(FilePath, json);
        }

        public static Settlement LoadData()
        {
            if (!File.Exists(FilePath)) return new Settlement();

            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<SettlementDTO>(json, _settings);
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
                    Polygon = plot.Description.Polygon
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

    public class PointConverter : JsonConverter<Point>
    {
        public override Point ReadJson(JsonReader reader, Type objectType, Point existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var str = reader.Value?.ToString();
            if (string.IsNullOrEmpty(str)) return new Point(0, 0);

            var parts = str.Split(';');
            if (parts.Length != 2 ||
                !double.TryParse(parts[0], out double x) ||
                !double.TryParse(parts[1], out double y))
                return new Point(0, 0);
            //
            return new Point(x, y);
        }

        public override void WriteJson(JsonWriter writer, Point value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.X};{value.Y}");
        }
    }
}
