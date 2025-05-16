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
    /// Interaction logic for AddSettlementWindow.xaml
    /// </summary>
    public partial class AddSettlementWindow : Window
    {
        public string SettlementName { get; private set; }

        public AddSettlementWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SettlementName = NameTextBox.Text;
            if (string.IsNullOrWhiteSpace(SettlementName))
            {
                MessageBox.Show("Введіть назву населеного пункту!");
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
