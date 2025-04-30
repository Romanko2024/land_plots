using LandManagementApp.Models;
using LandManagementApp.ViewModels;
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
using System.Windows.Shapes;

namespace LandManagementApp.Views
{
    /// <summary>
    /// Interaction logic for EditDescriptionWindow.xaml
    /// </summary>
    public partial class EditDescriptionWindow : Window
    {
        public EditDescriptionWindow(Description description)
        {
            InitializeComponent();
            DataContext = new EditDescriptionViewModel(description);
        }
        public EditDescriptionViewModel ViewModel => (EditDescriptionViewModel)DataContext;
    }
}
