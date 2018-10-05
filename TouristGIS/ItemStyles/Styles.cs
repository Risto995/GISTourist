using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TouristGIS.ItemStyles
{
    public interface Styles
    {
        void SetSymbol(int styleType);
        Symbol GetSymbol(Color color, Enum marker, string classCode = null);
        Enum GetSymbol();

        List<Enum> GetList();
    }
}
