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
    public partial class EditOwnerViewModel : ObservableObject
    {
        [ObservableProperty]
        private Owner _currentOwner;

        public EditOwnerViewModel(Owner owner)
        {
            CurrentOwner = owner.Clone(); //клонування для безпечного редагування
        }

        [RelayCommand]
        private void Save(Window window)
        {
            if (CurrentOwner.HasErrors) //перевірка валідації
            {
                MessageBox.Show("Виправте помилки введення.");
                return;
            }
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
