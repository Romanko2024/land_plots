using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LandManagementApp.Models;
using System.Windows;
using LandManagementApp.Views;
using LandManagementApp.ViewModels;

namespace LandManagementApp.ViewModels
{
    // ViewModel для вікна редагування земельної ділянки.
    // відповідає за логіку взаємодії з формою редагування.
    public partial class EditLandPlotViewModel : ObservableObject
    {
        [ObservableProperty]
        private LandPlot _currentPlot;

        public Array PurposeTypes => Enum.GetValues(typeof(PurposeType));

        public EditLandPlotViewModel(LandPlot plot)
        {
            _currentPlot = plot ?? new LandPlot();
        }

        [RelayCommand]
        private void EditOwner()
        {
            var editWindow = new Views.EditOwnerWindow(CurrentPlot.Owner);
            if (editWindow.ShowDialog() == true)
            {
                OnPropertyChanged(nameof(CurrentPlot));
            }
        }

        [RelayCommand]
        private void EditDescription()
        {
            var editWindow = new Views.EditDescriptionWindow(CurrentPlot.Description);
            if (editWindow.ShowDialog() == true)
            {
                OnPropertyChanged(nameof(CurrentPlot));
            }
        }

        [RelayCommand]
        private void Save(Window window)
        {
            //перевірка валідності даних
            var errors = new List<string>();

            if (CurrentPlot.Owner?.HasErrors ?? true)
                errors.Add("Помилки у власнику");
            if (CurrentPlot.Description?.HasErrors ?? true)
                errors.Add("Помилки у описі");
            if (CurrentPlot.MarketValue <= 0)
                errors.Add("Некоректна вартість");

            if (errors.Any())
            {
                MessageBox.Show($"Знайдено помилки:\n{string.Join("\n", errors)}");
                return;
            }

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