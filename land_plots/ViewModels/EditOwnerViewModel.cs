using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LandManagementApp.Models;
using System.Windows;

namespace LandManagementApp.ViewModels
{
    public partial class EditOwnerViewModel : ObservableObject
    {
        [ObservableProperty]
        private Owner _currentOwner;

        public EditOwnerViewModel(Owner owner)
        {
            CurrentOwner = owner ?? new Owner(); //клонування для безпечного редагування
        }

        [RelayCommand]
        private void Save(Window window)
        {
            // перевірка чи передано коректне посилання на вікно
            if (window == null)
            {
                MessageBox.Show("Помилка: вікно не знайдено.");
                return;
            }

            //чи ініціалізовано CurrentOwner
            if (CurrentOwner == null)
            {
                MessageBox.Show("Помилка: дані власника відсутні.");
                return;
            }

            //валідація даних
            if (CurrentOwner.HasErrors)
            {
                MessageBox.Show("Виправте помилки перед збереженням.");
                return;
            }

            try
            {
                window.DialogResult = true;
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
