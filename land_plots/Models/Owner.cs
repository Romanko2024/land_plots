using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace land_plots.Models
{
    public class Owner
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        public Owner(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        [Required(ErrorMessage = "Ім'я обов'язкове")]
        public string FirstName
        {
            get => _firstName;
            set => _firstName = !string.IsNullOrWhiteSpace(value)
                ? value : throw new ArgumentException("Ім'я не може бути порожнім.");
        }

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        public string LastName
        {
            get => _lastName;
            set => _lastName = !string.IsNullOrWhiteSpace(value)
                ? value : throw new ArgumentException("Прізвище не може бути порожнім.");
        }

        [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Невірна дата")]
        public DateTime BirthDate { get; set; }
    }
}
