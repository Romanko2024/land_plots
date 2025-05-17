using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows; //для Point
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Collections.Specialized;

namespace LandManagementApp.Models
{
    public class Description : INotifyPropertyChanged
    {
        //поля для збереження значень рівня води та полігону
        private int _groundWaterLevel;
        private ObservableCollection<ObservablePoint> _polygon = new ObservableCollection<ObservablePoint>();
        public PointCollection PolygonAsPoints
        {
            get
            {
                var points = Polygon.Select(p => new Point(p.X, p.Y)).ToList();
                return new PointCollection(points);
            }
        }

        //для створення об'єкта без ініціалізації
        public Description()
        {
            _polygon = new ObservableCollection<ObservablePoint>();
        }

        public Description(int groundWaterLevel, IEnumerable<Point> polygon)
        {
            GroundWaterLevel = groundWaterLevel;
            _polygon = new ObservableCollection<ObservablePoint>(
                polygon?.Select(p => ObservablePoint.FromPoint(p)) ?? new List<ObservablePoint>());
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
                    OnPropertyChanged(nameof(IsValid)); //+ оновлення валідації
                }
            }
        }
        //список точок, де кожна точка є об'єктом класу Point
        public ObservableCollection<ObservablePoint> Polygon
        {
            get => _polygon;
            set
            {
                //чи кількість точок більше або дорівнює 3
                if (_polygon != null)
                {
                    _polygon.CollectionChanged -= Polygon_CollectionChanged;
                    foreach (var point in _polygon)
                        point.PropertyChanged -= Point_PropertyChanged;
                }

                _polygon = value ?? new ObservableCollection<ObservablePoint>();

                _polygon.CollectionChanged += Polygon_CollectionChanged;
                foreach (var point in _polygon)
                    point.PropertyChanged += Point_PropertyChanged;

                OnPropertyChanged(nameof(Polygon));
                OnPropertyChanged(nameof(IsValid)); // + оновлення валідації
            }
        }
        private void Polygon_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsValid));
        }
        private void Point_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsValid));
        }
        //реалізац INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasErrors => !IsValid;

        public Description Clone()
        {
            return new Description
            {
                GroundWaterLevel = this.GroundWaterLevel,
                Polygon = new ObservableCollection<ObservablePoint>(
                    this.Polygon.Select(p => new ObservablePoint { X = p.X, Y = p.Y }))
            };
        }
        public bool IsValid =>
            _polygon != null &&
            _polygon.Count >= 3 &&
            GroundWaterLevel >= 0;
        // сповіщення про зміни властивостей класу
        protected virtual void OnPropertyChanged(string propertyName)
        {
            //якщоє підписники події то викликаємо їх з передачею зміненої властивості
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
