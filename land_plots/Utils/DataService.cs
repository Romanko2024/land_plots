using LandManagementApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LandManagementApp.Utils
{
    //клас для збереження та завантаження об'єкта Settlement у файл
    public static class DataService
    {
        // шлях до файлу для збер/завантаження даних
        private const string FilePath = "data.json";

        //метод збереження об'єкта Settlement у файл у форматі JSON
        public static void SaveData(Settlement settlement)
        {
            //серіалізуємо об'єкт Settlement у формат JSON з *красивим* (Indented) форматуванням
            var json = JsonConvert.SerializeObject(settlement, Formatting.Indented);

            //записуємо JSON-рядок у файл
            File.WriteAllText(FilePath, json);
        }

        //метод завантаження об'єкта Settlement із файлу
        public static Settlement LoadData()
        {
            //if файл не існує, повертаємо новий об'єкт Settlement
            if (!File.Exists(FilePath)) return new Settlement();

            //зчитуємо вміст файлу JSON
            var json = File.ReadAllText(FilePath);

            //десеріалізуємо JSON-рядок назад у об'єкт Settlement
            return JsonConvert.DeserializeObject<Settlement>(json);
        }
    }
}
