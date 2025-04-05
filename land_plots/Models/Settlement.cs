using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LandManagementApp.Models
{
    public class Settlement : INotifyPropertyChanged
    {
        //змінна для відстеження кількості створених Settlement
        //вик для генерації унікального номера
        private static int _totalCount;
        //поле для зберігання земельних ділянок
        private List<LandPlot> _landPlots = new List<LandPlot>();
        // при створенні нового Settlement автоматично генерується порядковий номер
        public Settlement() => SerialNumber = ++_totalCount;
        // унікальний номер населеного пункту 
        public int SerialNumber { get; }

        // список земельних ділянок пов’язаних з цим населеним пунктом
        public List<LandPlot> LandPlots
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

        //додавання нової земельної ділянки до списку
        public void AddLandPlot(LandPlot plot)
        {
            // Перевіряє, чи перетинається нова ділянка з уже наявними
            if (LandPlots.Any(lp => PolygonOverlap.Check(lp.Description.Polygon, plot.Description.Polygon))) //зробити PolygonOverlap
                throw new InvalidOperationException("Ділянки перетинаються!");

            //якщо не перетинаються — додає ділянку до списку
            LandPlots.Add(plot);
            OnPropertyChanged(nameof(LandPlots));
        }

        //коротка інф
        public string GetSummary() => $"Населений пункт №{SerialNumber}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
