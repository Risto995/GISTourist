using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouristGIS.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Map map = new Map();
        public Map Map
        {
            get { return map; }
            set { map = value; OnPropertyChanged(); }
        }

        private ObservableCollection<LayerListItem> loadedLayers = new ObservableCollection<LayerListItem>();
        public ObservableCollection<LayerListItem> LoadedLayers
        {
            get
            {
                return loadedLayers;
            }
            set
            {
                loadedLayers = value;
                OnPropertyChanged();
            }
        }

        private Layer selectedLayer;
        public Layer SelectedLayer
        {
            get
            {
                return selectedLayer;
            }
            set
            {
                selectedLayer = value;
                OnPropertyChanged();
            }
        }

        private PropertyViewModel layerProperty;
        public PropertyViewModel LayerProperty
        {
            get
            {
                return layerProperty;
            }
            set
            {
                layerProperty = value;
                OnPropertyChanged();
            }
        }

        private IDictionary<string, object> selectedFeature { get; set; }
        public IDictionary<string, object> SelectedFeature
        {
            get
            {
                return selectedFeature;
            }
            set
            {
                selectedFeature = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
