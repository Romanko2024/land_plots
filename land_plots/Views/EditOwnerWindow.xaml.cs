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
using LandManagementApp.Models;
using LandManagementApp.ViewModels;

namespace LandManagementApp.Views
{
    public partial class EditOwnerWindow : Window
    {
        public EditOwnerWindow(Owner owner)
        {
            InitializeComponent();
            DataContext = new EditOwnerViewModel(owner);
        }
    }
}
