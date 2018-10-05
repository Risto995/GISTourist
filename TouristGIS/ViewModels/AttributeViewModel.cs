using Esri.ArcGISRuntime.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouristGIS.ViewModels
{
    public class AttributeViewModel : INotifyPropertyChanged
    {
        public AttributeViewModel()
        {
            InTable = true;
        }
        public FeatureLayer SourceLayer { get; set; }
        private string filter;
        public string Filter
        {
            get
            { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
            }
        }
        public bool InTable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
