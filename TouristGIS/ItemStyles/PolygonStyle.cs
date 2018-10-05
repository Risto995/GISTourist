using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TouristGIS.ItemStyles
{
    public class PolygonStyle : Styles
    {
        public Enum SelectedType { get; set; }
        public PolygonStyle()
        {
            SelectedType = SimpleFillStyle.Solid;
        }
        public Symbol GetSymbol(Color color, Enum marker, string classCode = null)
        {
            var t = new SimpleFillSymbol()
            {
                Color = color,
                Style = (SimpleFillStyle)marker,
            };

            return t;
        }

        public void SetSymbol(int styleType)
        {
            SelectedType = (SimpleFillStyle)styleType;
        }

        public Enum GetSymbol()
        {
            return SelectedType;
        }

        public List<Enum> GetList()
        {
            return Enum.GetValues(typeof(SimpleFillStyle)).Cast<Enum>().ToList();
        }
    }
}
