using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Linq;
using TouristGIS.ViewModels;

namespace TouristGIS.Filters
{
    public class MeasureDistance
    {
        MapView MyMapView;
        MainViewModel model;
        GraphicsOverlay graphicsOverlay;

        Feature firstSelectedFeature;
        FeatureLayer firstLayer;
        Feature secondSelectedFeature;
        FeatureLayer secondLayer;

        public MeasureDistance(MapView MapView, MainViewModel model)
        {
            MyMapView = MapView;
            this.model = model;

            graphicsOverlay = new GraphicsOverlay();
            MyMapView.GraphicsOverlays.Add(graphicsOverlay);
        }

        public async void MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            FeatureLayer layer = model.SelectedLayer as FeatureLayer;
            if (layer == null)
                return;

            try
            {
                var rows = await layer.HitTestAsync(MyMapView, e.Position);
                if (rows != null && rows.Length > 0)
                {
                    layer.SelectFeatures(rows);
                    var features = await layer.FeatureTable.QueryAsync(rows);
                    var feature = features.FirstOrDefault();

                    if (firstSelectedFeature == null)
                    {
                        firstLayer = layer;
                        firstSelectedFeature = feature;
                    }
                    else
                    {
                        secondLayer = layer;
                        secondSelectedFeature = feature;

                        CalculateDistance();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void CalculateDistance()
        {
            if (firstSelectedFeature != null && secondSelectedFeature != null)
            {
                double distance = GeometryEngine.Distance(firstSelectedFeature.Geometry, secondSelectedFeature.Geometry);
                System.Windows.MessageBox.Show("Distance between two selected objects is: " + Math.Round(distance, 2) + " meters");
            }
        }

        internal void ClearGraphics()
        {
            firstLayer.ClearSelection();
            secondLayer.ClearSelection();

            firstSelectedFeature = null;
            secondSelectedFeature = null;
        }
    }
}
