using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel; //для ObservableObject і атрибутів MVVM Toolkit
using CommunityToolkit.Mvvm.Input; //для RelayCommand
using LandManagementApp.Models;
using System.Collections.ObjectModel; //для ObservableCollection
using LandManagementApp.Utils;
using System.Windows;
using LandManagementApp.Views;

namespace LandManagementApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Settlement> _settlements = new();

        [ObservableProperty]
        private Settlement? _selectedSettlement;

        [ObservableProperty]
        private LandPlot? _selectedPlot;

        public ObservableCollection<LandPlot> CurrentLandPlots =>
            new ObservableCollection<LandPlot>(_selectedSettlement?.LandPlots ?? new List<LandPlot>());

        public MainViewModel()
        {
            // Завантаження всіх населених пунктів
            var loadedData = DataService.LoadSettlements();
            if (loadedData != null)
            {
                _settlements = new ObservableCollection<Settlement>(loadedData);
                if (_settlements.Any())
                    SelectedSettlement = _settlements.First();
            }
        }

        [RelayCommand]
        private void AddSettlement()
        {
            var newSettlement = new Settlement();
            _settlements.Add(newSettlement);
            SelectedSettlement = newSettlement;
        }

        [RelayCommand]
        private void AddPlot()
        {
            if (SelectedSettlement == null)
            {
                MessageBox.Show("Оберіть населений пункт!");
                return;
            }

            var newPlot = new LandPlot();
            var editWindow = new EditLandPlotWindow(newPlot);

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    SelectedSettlement.AddLandPlot(newPlot);
                    OnPropertyChanged(nameof(CurrentLandPlots));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [RelayCommand(CanExecute = nameof(CanEditPlot))]
        private void EditPlot()
        {
            if (SelectedPlot == null) return;

            try
            {
                var clone = SelectedPlot.Clone();
                var editWindow = new EditLandPlotWindow(clone);

                if (editWindow.ShowDialog() == true)
                {
                    SelectedSettlement!.LandPlots.Remove(SelectedPlot);
                    SelectedSettlement.AddLandPlot(clone);
                    OnPropertyChanged(nameof(CurrentLandPlots));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування: {ex.Message}");
            }
        }

        [RelayCommand]
        private void SaveAndExit()
        {
            DataService.SaveSettlements(_settlements.ToList());
            Application.Current.Shutdown();
        }

        private bool CanEditPlot() => SelectedPlot != null;
    }
}
