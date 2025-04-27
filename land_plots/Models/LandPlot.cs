using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LandManagementApp.Models
{
    public class LandPlot : INotifyPropertyChanged
    {
        //поля для властивостей
        private Owner _owner;
        private Description _description;
        private PurposeType _purpose;
        private decimal _marketValue;

        public LandPlot() { }
        public LandPlot(Owner owner, Description description, PurposeType purpose, decimal marketValue)
        {
            Owner = owner;
            Description = description;
            Purpose = purpose;
            MarketValue = marketValue;
        }

        [Required(ErrorMessage = "Власник обов'язковий")]
        public Owner Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    //якщо передано null — кидаємо виняток
                    _owner = value ?? throw new ArgumentNullException(nameof(value));
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }

        [Required(ErrorMessage = "Опис обов'язковий")]
        public Description Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value ?? throw new ArgumentNullException(nameof(value));
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        //Purpose(enum) не обов'язкове для валідації
        public PurposeType Purpose
        {
            get => _purpose;
            set
            {
                if (_purpose != value)
                {
                    _purpose = value;
                    OnPropertyChanged(nameof(Purpose));
                }
            }
        }

        //вартість має бути додатньою
        [Range(0.01, double.MaxValue, ErrorMessage = "Вартість має бути додатньою")]
        public decimal MarketValue
        {
            get => _marketValue;
            set
            {
                if (value != _marketValue)
                {
                    _marketValue = value > 0 ? value
                        : throw new ArgumentOutOfRangeException(nameof(value));
                    OnPropertyChanged(nameof(MarketValue));
                }
            }
        }

        //метод КОРОТКОГО представлення у вигляді тексту
        public string GetSummary() => $"{Owner.LastName} - {MarketValue:C}";
        public LandPlot Clone()
        {
            return new LandPlot(
                new Owner(this.Owner.FirstName, this.Owner.LastName, this.Owner.BirthDate),
                new Description(
                    this.Description.GroundWaterLevel,
                    new List<Point>(this.Description.Polygon) // Глибока копія списку точок
                ),
                this.Purpose,
                this.MarketValue
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //викликає подію PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
