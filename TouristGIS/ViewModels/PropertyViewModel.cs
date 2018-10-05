using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using TouristGIS.ItemStyles;

namespace TouristGIS.ViewModels
{
    public class PropertyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyViewModel() { }

        public Styles StyleInstance { get; set; }

        public Enum SelectedMarker { get; set; }

        public List<Enum> AttributeMarkers { get; set; }

        public string SelectedAttribute { get; set; }

        public List<string> Attributes { get; set; }

        public Color SelectedColor { get; set; }

        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
