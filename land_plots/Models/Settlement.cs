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
        public static int GetCurrentCounter() => _totalCount;
        private List<LandPlot> _landPlots = new List<LandPlot>(); //список ділянок
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static void ResetCounter(int startValue = 0)
        {
            _totalCount = startValue;
        }
        public int SerialNumber { get; } // Автоматичний порядковий номер
        public List<LandPlot> LandPlots => _landPlots;
        //автомат генер порядковий номер
        public Settlement() => SerialNumber = ++_totalCount;
        public Settlement(int serialNumber)
        {
            SerialNumber = serialNumber;
            _totalCount = Math.Max(_totalCount, serialNumber);
        }

        //методи для керування ділянками
        public void AddLandPlot(LandPlot plot)
        {
            if (plot == null)
                throw new ArgumentNullException(nameof(plot));

            if (plot.Settlement != this)
                plot.Settlement = this; //прив'язуємо ділянку до поточного населеного пункту

            //перевірка на перетин з іншими ділянками
            if (LandPlots.Any(lp => PolygonOverlap.Check(lp.Description.Polygon, plot.Description.Polygon)))
                throw new InvalidOperationException("Ділянки перетинаються!");

            _landPlots.Add(plot);
        }

        public void RemoveLandPlot(LandPlot plot)
        {
            if (plot == null) return;

            plot.Settlement = null;
            _landPlots.Remove(plot);
        }
    }
}
