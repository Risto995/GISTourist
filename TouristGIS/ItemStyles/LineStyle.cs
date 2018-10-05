using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TouristGIS.ItemStyles
{
    public class LineStyle : Styles
    {
        public Enum SelectedType { get; set; }

        public LineStyle()
        {
            SelectedType = SimpleLineStyle.Solid;
        }
        public Symbol GetSymbol(Color color, Enum marker, string classCode = null)
        {
            var t = new SimpleLineSymbol()
            {
                Color = color,
                Style = (SimpleLineStyle)marker,
                Width = 3
            };

            return t;
        }

        public void SetSymbol(int styleType)
        {
            SelectedType = (SimpleLineStyle)styleType;
        }

        public Enum GetSymbol()
        {
            return SelectedType;
        }

        public List<Enum> GetList()
        {
            return Enum.GetValues(typeof(SimpleLineStyle)).Cast<Enum>().ToList();
        }
    }
}

