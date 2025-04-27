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
    // ViewModel головного вікна яка працює з земельними ділянками та їх збереженням
    public partial class MainViewModel : ObservableObject
    {
        // колекція ділянок для відображення в інтерфейсі (ObservableCollection автоматично оновлює GUI при змінах)
        [ObservableProperty]
        private ObservableCollection<LandPlot> _landPlots = new ObservableCollection<LandPlot>();

        //поточна вибрана ділянка
        [ObservableProperty]
        private LandPlot _selectedPlot;

        //поточний населений пункт із завантаженими даними
        [ObservableProperty]
        private Settlement _currentSettlement = DataService.LoadData();

        // команда для додавання нової земельної ділянки
        [RelayCommand]
        private void AddPlot()
        {
            // створюємо нову порожню ділянку
            var newPlot = new LandPlot();
            // відкриваємо вікно редагування ділянки
            var editWindow = new LandManagementApp.Views.EditLandPlotWindow(newPlot);
            // якщо користувач натиснув "ОК" (підтвердив введення)
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    // додаємо ділянку в поточний населений пункт
                    _currentSettlement.AddLandPlot(newPlot);
                    //додаємо ділянку в колекцію для відображення в GUI
                    LandPlots.Add(newPlot);
                }
                catch (Exception ex)
                {
                    // якщо помилка (напр перетин полігонів)
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // команда для збереження даних і закриття додатку
        [RelayCommand]
        private void SaveAndExit()
        {
            // зберігаємо поточний стан н.п. в файл
            DataService.SaveData(_currentSettlement);
            // закриваємо додаток
            Application.Current.Shutdown();
        }
    }
}
