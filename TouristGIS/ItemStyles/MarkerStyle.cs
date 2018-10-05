using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TouristGIS.ItemStyles
{
    public class MarkerStyle : Styles
    {
        public Enum SelectedType { get; set; }
        public MarkerStyle()
        {
            SelectedType = SimpleMarkerStyle.Circle;
        }
        public Symbol GetSymbol(Color color, Enum marker, string classCode = null)
        {
            var stickPinSymbol = new PictureMarkerSymbol() { Width = 30, Height = 30, XOffset = 0, YOffset = 0 };

            if (classCode != null)
            {
                var uri = new Uri($"pack://application:,,,/Images/{classCode}.png");
                stickPinSymbol.SetSourceAsync(uri);
                //stickPinSymbol.YOffset = 25;
                return stickPinSymbol;
            }

            return new SimpleMarkerSymbol()
            {
                Size = 15,
                Color = color,
                Style = (SimpleMarkerStyle)marker,
            };
        }

        public void SetSymbol(int styleType)
        {
            SelectedType = (SimpleMarkerStyle)styleType;
        }

        public Enum GetSymbol()
        {
            return SelectedType;
        }

        public List<Enum> GetList()
        {
            return Enum.GetValues(typeof(SimpleMarkerStyle)).Cast<Enum>().ToList();
        }
    }
}
