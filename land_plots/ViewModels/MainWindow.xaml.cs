using LandManagementApp.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LandManagementApp.ViewModels;
using LandManagementApp.Utils;
using System.ComponentModel;

namespace LandManagementApp.ViewModels
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel == null) return;

            var result = MessageBox.Show(
                "Зберегти зміни перед виходом?",
                "Підтвердження",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    DataService.SaveData(viewModel.CurrentSettlement);
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
