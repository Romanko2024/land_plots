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
    public partial class EditDescriptionViewModel : ObservableObject
    {
        [ObservableProperty]
        private Description _currentDescription;

        public EditDescriptionViewModel(Description description)
        {
            CurrentDescription = description?.Clone() ?? new Description();
        }
        [RelayCommand]
        private void AddPoint()
        {
            CurrentDescription.Polygon.Add(new Point(10, 20));
            OnPropertyChanged(nameof(CurrentDescription.IsValid));
        }
        [RelayCommand]
        private void Save(Window window)
        {
            if (CurrentDescription == null)
            {
                MessageBox.Show("Помилка: опис не ініціалізовано!");
                return;
            }
            if (!CurrentDescription.IsValid)
            {
                MessageBox.Show("Додайте щонайменше 3 точки для полігону!");
                return;
            }

            //
            //
            window.DialogResult = true;
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
