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
    public class LandPlot : INotifyPropertyChanged, IDataErrorInfo
    {
        //поля для властивостей
        private Owner _owner;
        private Description _description;
        private PurposeType _purpose;
        private decimal _marketValue;

        public LandPlot()
        {
            //ініціалізуємо обов'язкові поля за замовчуванням
            _owner = new Owner();
            _description = new Description();
        }

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

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335",
            ErrorMessage = "Вартість має бути додатною")]
        public decimal MarketValue
        {
            get => _marketValue;
            set
            {
                if (value != _marketValue)
                {
                    _marketValue = value;
                    OnPropertyChanged(nameof(MarketValue));
                }
            }
        }

        //метод КОРОТКОГО представлення у вигляді тексту
        public string GetSummary() => $"{Owner.LastName} - {MarketValue:C}";

        public LandPlot Clone()
        {
            return new LandPlot(
                Owner.Clone(),
                Description.Clone(),
                Purpose,
                MarketValue
            );
        }

        public bool HasErrors =>
            (Owner?.HasErrors ?? true) ||  // Перевірка на null
            (Description?.HasErrors ?? true) ||
            MarketValue <= 0;

        public event PropertyChangedEventHandler PropertyChanged;

        //викликає подію PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Owner):
                        return Owner?.HasErrors == true ? "Некоректні дані власника" : null;

                    case nameof(Description):
                        return Description?.HasErrors == true ? "Некоректний опис" : null;

                    case nameof(MarketValue):
                        return MarketValue <= 0 ? "Вартість має бути додатною" : null;

                    default:
                        return null;
                }
            }
        }
        public string Error => null;
    }
}
