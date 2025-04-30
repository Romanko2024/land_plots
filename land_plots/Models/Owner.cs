using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LandManagementApp.Models
{
    [DataContract] //для серіалізації
    //щоб зберігати у форматі JSON/XML
    public class Owner : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        //подія для оновлення GUI
        public event PropertyChangedEventHandler PropertyChanged;

        //ПОРОЖНІЙ КОНСТРУКТОР ПОТРІБЕН ДЛЯ СЕРІАЛІЗАЦІЇЇ
        public Owner() { }

        public Owner(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        [DataMember]
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = !string.IsNullOrWhiteSpace(value)
                        ? value
                        : throw new ArgumentException("Ім'я не може бути порожнім.");
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        [DataMember]
        [Required(ErrorMessage = "Прізвище обов'язкове")]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = !string.IsNullOrWhiteSpace(value)
                        ? value
                        : throw new ArgumentException("Прізвище не може бути порожнім.");
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        [DataMember]
        [CustomValidation(typeof(Owner), nameof(ValidateBirthDate))]
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public static ValidationResult ValidateBirthDate(DateTime date)
        {
            return (date.Year >= 1900 && date.Year <= 2100)
                ? ValidationResult.Success
                : new ValidationResult("Допустимі роки: 1900-2100");
        }

        //INotifyPropertyChanged оновлює GUI при зміні властивостей
        protected virtual void OnPropertyChanged(string propertyName)//метод інвокить подію
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Owner Clone()
        {
            return new Owner(this.FirstName, this.LastName, this.BirthDate);
        }
        //інтерфейс IDataErrorInfo
        //щоб не давало загальну помилку
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FirstName):
                        return string.IsNullOrEmpty(FirstName) ? "Ім'я обов'язкове" : null;
                    case nameof(LastName):
                        return string.IsNullOrEmpty(LastName) ? "Прізвище обов'язкове" : null;
                    case nameof(BirthDate):
                        return ValidateBirthDate(BirthDate).ErrorMessage;
                    default:
                        return null;
                }
            }
        }
    }
}
