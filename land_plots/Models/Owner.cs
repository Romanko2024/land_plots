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
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [DataMember]
        [Required(ErrorMessage = "Прізвище обов'язкове")]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        [DataMember]
        [CustomValidation(typeof(Owner), nameof(ValidateBirthDate))]
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate == value) return;
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public static ValidationResult ValidateBirthDate(DateTime date)
        {
            if (date.Year < 1900 || date.Year > 2100)
                return new ValidationResult("Допустимі роки: 1900-2100");

            if (date > DateTime.Today.AddYears(-14))
                return new ValidationResult("Власник повинен бути старше 14 років");

            return ValidationResult.Success;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FirstName):
                        return string.IsNullOrWhiteSpace(FirstName)
                            ? "Ім'я обов'язкове"
                            : null;

                    case nameof(LastName):
                        return string.IsNullOrWhiteSpace(LastName)
                            ? "Прізвище обов'язкове"
                            : null;

                    case nameof(BirthDate):
                        return ValidateBirthDate(BirthDate).ErrorMessage;

                    default:
                        return null;
                }
            }
        }

        public bool HasErrors =>
            !string.IsNullOrEmpty(this[nameof(FirstName)]) ||
            !string.IsNullOrEmpty(this[nameof(LastName)]) ||
            !string.IsNullOrEmpty(this[nameof(BirthDate)]);

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Owner Clone()
        {
            return new Owner(FirstName, LastName, BirthDate);
        }
    }
}
