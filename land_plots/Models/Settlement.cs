using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LandManagementApp.Utils;

namespace LandManagementApp.Models
{
    public class Settlement : INotifyPropertyChanged
    {
        private static int _totalCount;
        public static void ResetCounter(int startValue = 0)
        {
            _totalCount = startValue;
        }
        //поле для зберігання земельних ділянок (використовуємо ObservableCollection)
        private ObservableCollection<LandPlot> _landPlots = new ObservableCollection<LandPlot>();

        //автомат генер порядковий номер
        public Settlement() => SerialNumber = ++_totalCount;

        public int SerialNumber { get; }

        //ЄДИНА властивість LandPlots тип ObservableCollection<LandPlot>
        public ObservableCollection<LandPlot> LandPlots
        {
            get => _landPlots;
            set
            {
                if (_landPlots != value)
                {
                    _landPlots = value;
                    OnPropertyChanged(nameof(LandPlots));
                }
            }
        }

        //додавання ділянки
        public void AddLandPlot(LandPlot plot)
        {
            // Перевіряє, чи перетинається нова ділянка з уже наявними
            if (LandPlots.Any(lp =>
        PolygonOverlap.Check(
            lp.Description.Polygon, // Без конвертації
            plot.Description.Polygon
        )))
    throw new InvalidOperationException("Ділянки перетинаються!");
            LandPlots.Add(plot);
            OnPropertyChanged(nameof(LandPlots));
        }

        public string GetSummary() => $"Населений пункт №{SerialNumber}";

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
