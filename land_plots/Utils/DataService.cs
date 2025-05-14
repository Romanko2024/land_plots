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
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects //для коректної серіалізації колекцій
        };

        public static void SaveSettlements(List<Settlement> settlements)
        {
            // Конвертація Settlement -> SettlementDTO
            try
            {
                var dtos = settlements.Select(ConvertToSettlementDTO).ToList();
                var json = JsonConvert.SerializeObject(dtos, _settings);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        //завантаження всіх населених пунктів
        public static List<Settlement> LoadSettlements()
        {
            try
            {
                if (!File.Exists(FilePath)) return new List<Settlement>();

                var json = File.ReadAllText(FilePath);
                var dtos = JsonConvert.DeserializeObject<List<SettlementDTO>>(json, _settings);

                //оновлення лічильника
                if (dtos.Any())
                {
                    var maxSerial = dtos.Max(s => s.SerialNumber);
                    Settlement.ResetCounter(maxSerial);
                }

                return dtos.Select(ConvertFromSettlementDTO).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
                return new List<Settlement>();
            }
        }

        //конверт моделей <-> DTO

        private static SettlementDTO ConvertToSettlementDTO(Settlement settlement)
        {
            return new SettlementDTO
            {
                SerialNumber = settlement.SerialNumber,
                LandPlots = settlement.LandPlots.Select(ConvertToLandPlotDTO).ToList()
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
                    Polygon = plot.Description.Polygon.Select(op => op.ToPoint()).ToList()
                },
                Purpose = plot.Purpose,
                MarketValue = plot.MarketValue
            };
        }

        private static LandPlot ConvertFromLandPlotDTO(LandPlotDTO dto)
        {
            var owner = new Owner(dto.Owner.FirstName, dto.Owner.LastName, dto.Owner.BirthDate);
            var description = new Description(
                dto.Description.GroundWaterLevel,
                dto.Description.Polygon
            );
            return new LandPlot(owner, description, dto.Purpose, dto.MarketValue);
        }

        //

        public class PointConverter : JsonConverter<ObservablePoint>
        {
            public override ObservablePoint ReadJson(
                JsonReader reader,
                Type objectType,
                ObservablePoint existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                var str = reader.Value?.ToString();
                if (string.IsNullOrEmpty(str)) return new ObservablePoint { X = 0, Y = 0 };

                var parts = str.Split(';');
                if (parts.Length != 2 ||
                    !double.TryParse(parts[0], out double x) ||
                    !double.TryParse(parts[1], out double y))
                    return new ObservablePoint { X = 0, Y = 0 };

                return new ObservablePoint { X = x, Y = y };
            }

            public override void WriteJson(
                JsonWriter writer,
                ObservablePoint value,
                JsonSerializer serializer)
            {
                writer.WriteValue($"{value.X:0.##};{value.Y:0.##}");
            }
        }
    }
}
