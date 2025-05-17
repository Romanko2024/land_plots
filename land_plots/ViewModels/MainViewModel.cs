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

        partial void OnSelectedSettlementChanged(Settlement? value)
        {
            OnPropertyChanged(nameof(CurrentLandPlots));
            OnPropertyChanged(nameof(SelectedSettlement));
        }

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
            var dialog = new AddSettlementWindow();
            if (dialog.ShowDialog() == true)
            {
                var newSettlement = new Settlement { Name = dialog.SettlementName };
                Settlements.Add(newSettlement);
                SelectedSettlement = newSettlement;
            }
        }

        [RelayCommand]
        private void AddPlot()
        {
            if (!Settlements.Any())
            {
                MessageBox.Show("Спочатку додайте населений пункт!");
                return;
            }

            var newPlot = new LandPlot();
            var editWindow = new EditLandPlotWindow(newPlot, Settlements);

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    editWindow.ViewModel.SelectedSettlement.AddLandPlot(newPlot);
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
            if (SelectedPlot == null || SelectedPlot.Settlement == null)
            {
                MessageBox.Show("Оберіть ділянку та населений пункт!");
                return;
            }

            try
            {
                var clone = SelectedPlot.Clone();
                var editWindow = new EditLandPlotWindow(
                    clone,
                    Settlements,
                    SelectedPlot.Settlement
                );

                if (editWindow.ShowDialog() == true)
                {
                    //видаляємо стару ділянку з поточного населеного пункту
                    SelectedPlot.Settlement.RemoveLandPlot(SelectedPlot);

                    //+оновлену ділянку до нового населеного пункту
                    if (editWindow.ViewModel?.SelectedSettlement != null)
                    {
                        editWindow.ViewModel.SelectedSettlement.AddLandPlot(clone);
                    }

                    OnPropertyChanged(nameof(CurrentLandPlots));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування: {ex.Message}");
            }
        }
        partial void OnSelectedPlotChanged(LandPlot? value)
        {
            EditPlotCommand.NotifyCanExecuteChanged();
            DeletePlotCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void SaveAndExit()
        {
            DataService.SaveSettlements(_settlements.ToList());
            Application.Current.Shutdown();
        }

        private bool CanEditPlot() => SelectedPlot != null;

        [RelayCommand(CanExecute = nameof(CanDeletePlot))]
        private void DeletePlot()
        {
            if (SelectedPlot == null || SelectedSettlement == null) return;

            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити цю ділянку?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                SelectedSettlement.RemoveLandPlot(SelectedPlot);
                SelectedPlot = null;
                OnPropertyChanged(nameof(CurrentLandPlots));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення: {ex.Message}");
            }
        }

        private bool CanDeletePlot() => SelectedPlot != null && SelectedSettlement != null;
    }
}
