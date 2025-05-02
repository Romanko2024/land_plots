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

        public static IEnumerable<PurposeType> PurposeTypes =>
            Enum.GetValues(typeof(PurposeType)).Cast<PurposeType>();

        public EditLandPlotViewModel(LandPlot plot)
        {
            CurrentPlot = plot;
        }

        [RelayCommand]
        private void EditOwner()
        {
            var editWindow = new EditOwnerWindow(CurrentPlot.Owner);
            if (editWindow.ShowDialog() == true)
            {
                CurrentPlot.Owner = editWindow.ViewModel.CurrentOwner.Clone();
            }
        }

        [RelayCommand]
        private void EditDescription()
        {
            var editWindow = new EditDescriptionWindow(CurrentPlot.Description);
            if (editWindow.ShowDialog() == true)
            {
                CurrentPlot.Description = editWindow.ViewModel.CurrentDescription.Clone();
            }
        }

        [RelayCommand]
        private void Save(Window window)
        {
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