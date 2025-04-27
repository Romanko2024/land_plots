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
    // ViewModel для вікна редагування земельної ділянки.
    // відповідає за логіку взаємодії з формою редагування.
    public partial class EditLandPlotViewModel : ObservableObject
    {
        public static IEnumerable<PurposeType> PurposeTypes =>
        Enum.GetValues(typeof(PurposeType)).Cast<PurposeType>();
        // властивість, що представляє поточну ділянку, яку редагуємо.
        //[ObservableProperty] автоматично генерує код для сповіщень про зміни властивості.
        [ObservableProperty]
        private LandPlot _currentPlot;
        // Конструктор, який ініціалізує ViewModel з переданою ділянкою.
        //name="plot">Ділянка для редагування (або нова ділянка).
        public EditLandPlotViewModel(LandPlot plot)
        {
            CurrentPlot = plot; //призначаємо ділянку для редагування
        }
        //[RelayCommand] автомат створює команду яку можна прив'язати до кнопки в XAML.
        [RelayCommand]
        private void Save(Window window)
        {
            window?.Close(); 
        }
    }
}
