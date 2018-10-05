using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TouristGIS.Filters;
using TouristGIS.ViewModels;

namespace TouristGIS
{
    /// <summary>
    /// Interaction logic for AttributeFilterWindow.xaml
    /// </summary>
    public partial class AttributeFilterWindow : Window
    {
        MapView MyMapView;
        GraphicsOverlay graphicsOverlay;
        private AttributeViewModel attributeViewModel
        {
            get { return DataContext as AttributeViewModel; }
            set { DataContext = value; }
        }

        public AttributeFilterWindow(MapView myMapView)
        {
            MyMapView = myMapView;
            InitializeComponent();
        }

        private async void RunQuery_Click(object sender, RoutedEventArgs e)
        {
            string filter = attributeViewModel.Filter;
            IEnumerable<Feature> result = await AttributeFilter.DoAttributeFilter(attributeViewModel.SourceLayer, filter);

            if (attributeViewModel.InTable)
            {
                TableView tableView = new TableView(result);
                tableView.Top = 100;
                tableView.Left = 700;
                tableView.ShowDialog();
            }
            else
            {
                graphicsOverlay = new GraphicsOverlay() { ID = "resultsLayer" };
                graphicsOverlay.Graphics.Clear();

                foreach (var item in result)
                {
                    Graphic g = new Graphic(item.Geometry, GetSymbol(attributeViewModel.SourceLayer.FeatureTable.GeometryType));
                    graphicsOverlay.Graphics.Add(g);
                }

                MyMapView.GraphicsOverlays.Add(graphicsOverlay);
                attributeViewModel.SourceLayer.IsVisible = false;
            }
        }

        private Symbol GetSymbol(GeometryType geometryType)
        {
            switch (geometryType)
            {
                case GeometryType.Point:
                    return new SimpleMarkerSymbol();
                case GeometryType.Polyline:
                    return new SimpleLineSymbol();
                case GeometryType.Polygon:
                    return new SimpleFillSymbol();
                default:
                    return null;
            }
        }
    }
}
