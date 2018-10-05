using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TouristGIS.ViewModels
{
    public class SpatialViewModel : INotifyPropertyChanged
    {
        public SpatialViewModel()
        {
            Filters = Enum.GetValues(typeof(SpatialRelationship)).Cast<Enum>().ToList();
            SelectedRelation = SpatialRelationship.Contains;
            InTable = true;
            InRadius = true;
        }

        public LayerListItem SelectedSourceLayer { get; set; }
        public FeatureLayer SourceLayer
        {
            get { return SelectedSourceLayer.FLayer as FeatureLayer; }
            set { }
        }

        public LayerListItem SelectedDestinationLayer { get; set; }
        public FeatureLayer DestinationLayer
        {
            get { return SelectedDestinationLayer.FLayer as FeatureLayer; }
            set { }
        }

        private Visibility visible = Visibility.Collapsed;
        public Visibility Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<LayerListItem> LoadedLayers { get; set; }
        public List<Enum> Filters { get; set; }

        private string sourceFilter;
        public string SourceFilter
        {
            get
            {
                return sourceFilter;
            }
            set
            {
                sourceFilter = value;
                OnPropertyChanged();
            }
        }

        private string destinationFilter;
        public string DestinationFilter
        {
            get
            {
                return destinationFilter;
            }
            set
            {
                destinationFilter = value;
                OnPropertyChanged();
            }
        }

        public bool InRadius { get; set; }
        public int Radius { get; set; }
        public SpatialRelationship SelectedRelation { get; set; }
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
