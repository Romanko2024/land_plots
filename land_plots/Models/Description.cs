using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows; //для Point

namespace LandManagementApp.Models
{
    public class Description : INotifyPropertyChanged
    {
        //поля для збереження значень рівня води та полігону
        private int _groundWaterLevel;
        private List<Point> _polygon;

        //для створення об'єкта без ініціалізації
        public Description() { }
        public Description(int groundWaterLevel, List<Point> polygon)
        {
            GroundWaterLevel = groundWaterLevel;
            Polygon = polygon;
        }
        
        //атрибут Range забезпечує перевірку на допустимі значення.
        [Range(0, int.MaxValue, ErrorMessage = "Рівень води не може бути від'ємним")]
        public int GroundWaterLevel
        {
            get => _groundWaterLevel;
            set
            {
                // чи значення відрізняється від попереднього
                if (value != _groundWaterLevel)
                {
                    _groundWaterLevel = value >= 0 ? value
                        : throw new ArgumentOutOfRangeException(nameof(value));
                    //сповіщаємо зміну властивості
                    OnPropertyChanged(nameof(GroundWaterLevel));
                }
            }
        }
        //список точок, де кожна точка є об'єктом класу Point
        public List<Point> Polygon
        {
            get => _polygon;
            set
            {
                //чи кількість точок більше або дорівнює 3
                if (value?.Count >= 3)
                {
                    _polygon = value;
                    OnPropertyChanged(nameof(Polygon));
                }
                else
                {
                    throw new ArgumentException("Полігон має містити ≥3 точок.");
                }
            }
        }
        //реалізац INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // сповіщення про зміни властивостей класу
        protected virtual void OnPropertyChanged(string propertyName)
        {
            //якщоє підписники події то викликаємо їх з передачею зміненої властивості
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
