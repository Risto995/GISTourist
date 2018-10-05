using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Data;
using TouristGIS.ViewModels;
using TouristGIS.Filters;

namespace TouristGIS
{
    /// <summary>
    /// Interaction logic for SpatialFilterWindow.xaml
    /// </summary>
    public partial class SpatialFilterWindow : Window
    {
        MapView MyMapView;
        GraphicsOverlay graphicsOverlay;
        private SpatialViewModel spatialViewModel
        {
            get { return DataContext as SpatialViewModel; }
            set { DataContext = value; }
        }

        public SpatialFilterWindow(MapView myMapView)
        {
            MyMapView = myMapView;
            InitializeComponent();
        }

        private async void RunQuery_Click(object sender, RoutedEventArgs e)
        {
            SpatialFilter spatialFilter = new SpatialFilter();
            IEnumerable<Feature> result;
            string sourceQuery = spatialViewModel.SourceFilter;
            string destinationQuery = spatialViewModel.DestinationFilter;

            result = await spatialFilter.DoSpatialFilter(spatialViewModel.SourceLayer, spatialViewModel.DestinationLayer, spatialViewModel.Radius, spatialViewModel.SelectedRelation, sourceQuery, destinationQuery, spatialViewModel.InRadius);

            if (spatialViewModel.InTable)
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
                    Graphic g = new Graphic(item.Geometry, GetSymbol(spatialViewModel.DestinationLayer.FeatureTable.GeometryType));
                    graphicsOverlay.Graphics.Add(g);
                }

                MyMapView.GraphicsOverlays.Add(graphicsOverlay);
                spatialViewModel.SourceLayer.IsVisible = false;
                spatialViewModel.DestinationLayer.IsVisible = false;
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