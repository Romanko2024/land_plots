using LandManagementApp.Models;
using LandManagementApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandManagementApp.Views
{
    public partial class EditLandPlotWindow : Window
    {
        // конструктор, який приймає об'єкт LandPlot
        public EditLandPlotViewModel ViewModel => (EditLandPlotViewModel)DataContext;

        public EditLandPlotWindow(LandPlot plot, ObservableCollection<Settlement> settlements)
        {
            InitializeComponent();
            DataContext = new EditLandPlotViewModel(plot, settlements);
        }

        public EditLandPlotWindow(LandPlot plot, ObservableCollection<Settlement> settlements, Settlement currentSettlement)
        {
            InitializeComponent();
            DataContext = new EditLandPlotViewModel(plot, settlements, currentSettlement);
        }
    }
}
