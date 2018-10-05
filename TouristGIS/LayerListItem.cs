using Esri.ArcGISRuntime.Layers;
using TouristGIS.ViewModels;

namespace TouristGIS
{
    public class LayerListItem
    {
        public bool Checked { get; set; }
        public string Name { get; set; }
        public Layer FLayer { get; set; }

        public PropertyViewModel LayerProperty { get; set; }
    }
}
