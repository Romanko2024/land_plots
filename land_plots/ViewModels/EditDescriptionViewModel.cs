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
            CurrentDescription = description.Clone();
        }
        [RelayCommand]
        private void AddPoint()
        {
            CurrentDescription.Polygon.Add(new Point(0, 0));
            OnPropertyChanged(nameof(CurrentDescription.Polygon));
        }
        [RelayCommand]
        private void Save(Window window)
        {
            try
            {
                //виклик сеттера Polygon для валідації
                CurrentDescription.Polygon = new List<Point>(CurrentDescription.Polygon);

                if (CurrentDescription.HasErrors)
                    throw new ArgumentException("Некоректні дані");

                window.DialogResult = true;
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
