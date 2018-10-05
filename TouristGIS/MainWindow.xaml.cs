using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TouristGIS.Filters;
using TouristGIS.ItemStyles;
using TouristGIS.ViewModels;

namespace TouristGIS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MeasureDistance measureDistance;

        private MainViewModel mainViewModel
        {
            get { return DataContext as MainViewModel; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            OpenBasemapLayer();

            MyMapView.AllowDrop = true;
            MyMapView.DragOver += MyMapView_DragOver;
            MyMapView.Drop += MyMapView_Drop;
        }

        private void OpenBasemapLayer()
        {
            ArcGISTiledMapServiceLayer basemap = new ArcGISTiledMapServiceLayer(new Uri("http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer"));
            //ArcGISTiledMapServiceLayer basemap = new ArcGISTiledMapServiceLayer(new Uri("http://services.arcgisonline.com/ArcGIS/rest/services/Canvas/World_Light_Gray_Base/MapServer"));

            mainViewModel.Map.Layers.Clear();
            mainViewModel.Map.Layers.Add(basemap);
            mainViewModel.Map.InitialViewpoint = new Viewpoint(new MapPoint(500000, 4871873, SpatialReference.Create(32634)), 2500000);

            mainViewModel.LoadedLayers.Add(new LayerListItem() { Checked = true, Name = "Map", FLayer = basemap });
        }


        private async void OpenShapefileLayer(string path)
        {
            var shapefile = await ShapefileTable.OpenAsync(path);

            AttributeLabelClassCollection col = new AttributeLabelClassCollection();
            col.Add(new AttributeLabelClass()
            {
                TextExpression = attributesComboBox.Text,
                LabelPlacement = LabelPlacement.PointAboveCenter,
                Symbol = new TextSymbol()
                {
                    Color = Colors.Black,
                    Font = new SymbolFont()
                    {
                        FontFamily = "Times New Roman",
                        FontSize = 15,
                        FontWeight = SymbolFontWeight.Bold
                    }
                }
            });

            FeatureLayer flayer = new FeatureLayer(shapefile)
            {
                ID = shapefile.Name,
                DisplayName = path,
                Labeling = new LabelProperties()
                {
                    IsEnabled = true,
                    LabelClasses = col,
                },
                SelectionColor = Colors.Red,
            };

            var prop = LoadProperties(flayer);

            mainViewModel.Map.Layers.Add(flayer);
            var listItem = new LayerListItem() { Checked = true, Name = shapefile.Name, FLayer = flayer, LayerProperty = prop };
            mainViewModel.LoadedLayers.Add(listItem);

            BasemapListBox.SelectedItem = listItem;

            RefreshMap(flayer);

            txtInfo.DataContext = flayer;
            txtInfo.Visibility = Visibility.Visible;
        }

        private PropertyViewModel LoadProperties(FeatureLayer layer)
        {
            var model = new PropertyViewModel();

            model.Attributes = new List<string>();
            foreach (var filed in layer.FeatureTable.Schema.Fields)
            {
                if (filed.Name != "FID" && filed.Name != "shape")
                {
                    model.Attributes.Add(filed.Name);
                }
            }
            model.SelectedAttribute = "name";

            var styleInstance = GetStyleClass(layer.FeatureTable.GeometryType);
            model.StyleInstance = styleInstance;
            model.AttributeMarkers = model.StyleInstance.GetList();
            model.SelectedMarker = styleInstance.GetSymbol();

            model.SelectedColor = Colors.Black;

            return model;
        }

        private void RefreshMap(FeatureLayer layer)
        {
            if (layer == null)
                return;

            var labProp = mainViewModel.LayerProperty;
            layer.Labeling.LabelClasses = CreateLayerLabel(labProp);
            var styleClass = GetStyleClass(layer.FeatureTable.GeometryType);

            layer.Renderer = new SimpleRenderer()
            {
                Symbol = styleClass.GetSymbol(labProp.SelectedColor, labProp.SelectedMarker)
            };
        }


        #region helperMethods

        private Styles GetStyleClass(GeometryType type)
        {
            switch (type)
            {
                case GeometryType.Point:
                    return new MarkerStyle();
                case GeometryType.Polyline:
                    return new LineStyle();
                case GeometryType.Polygon:
                    return new PolygonStyle();
                default:
                    return null;
            }
        }

        private AttributeLabelClassCollection CreateLayerLabel(PropertyViewModel property = null)
        {
            AttributeLabelClassCollection col = new AttributeLabelClassCollection();
            col.Add(new AttributeLabelClass()
            {
                TextExpression = $"[{property.SelectedAttribute}]",
                LabelPlacement = LabelPlacement.PointBelowCenter,
                Symbol = new Esri.ArcGISRuntime.Symbology.TextSymbol()
                {
                    Color = property.SelectedColor,
                    Font = new SymbolFont()
                    {
                        FontFamily = "Times New Roman",
                        FontSize = 15,
                        FontWeight = SymbolFontWeight.Bold
                    }
                }
            });

            return col;
        }

        private Symbol GetSymbolIcon(string codeClass)
        {
            FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;

            var labProp = mainViewModel.LayerProperty;
            var layerType = layer.FeatureTable.GeometryType;
            var style = GetStyleClass(layerType);

            return style.GetSymbol(Colors.Transparent, labProp.SelectedMarker, codeClass);
        }

        #endregion helperMethods


        #region MyMapViewEvents

        private void MyMapView_LayerLoaded(object sender, LayerLoadedEventArgs e)
        {
            if (e.LoadError == null)
                return;

            Debug.WriteLine(string.Format("Error while loading layer : {0} - {1}", e.Layer.ID, e.LoadError.Message));
        }

        private void MyMapView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string f in files.Where(f => f.EndsWith(".shp")))
                    OpenShapefileLayer(f);
            }
        }

        private void MyMapView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any(f => f.EndsWith(".shp")))
                    return;
            }
            e.Handled = true;
        }

        private void MyMapView_MouseMove(object sender, MouseEventArgs e)
        {
            if (MyMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry) == null)
                return;

            Point screenPoint = e.GetPosition(MyMapView);

            MapPoint mapPoint = MyMapView.ScreenToLocation(screenPoint);

            if (MyMapView.WrapAround)
                mapPoint = GeometryEngine.NormalizeCentralMeridian(mapPoint) as MapPoint;

            var screenMapPoint = GeometryEngine.Project(mapPoint, new SpatialReference(4326)) as MapPoint;
            mapPoint = GeometryEngine.Project(mapPoint, new SpatialReference(31277)) as MapPoint;

            screenX.Text = "X: " + screenMapPoint.X;
            screenY.Text = "Y: " + screenMapPoint.Y;

            mapX.Text = "X: " + mapPoint.X;
            mapY.Text = "Y: " + mapPoint.Y;
        }

        private async void MyMapView_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
            if (layer == null)
                return;

            try
            {
                Feature result;
                var rows = await layer.HitTestAsync(MyMapView, e.Position);
                if (rows != null && rows.Length > 0)
                {
                    var features = await layer.FeatureTable.QueryAsync(rows);

                    FeatureOverlay.DataContext = features.FirstOrDefault();
                    MapView.SetViewOverlayAnchor(FeatureOverlay, e.Location);

                    result = features.FirstOrDefault();
                    layer.ClearSelection();
                    layer.SelectFeatures(rows);

                    mainViewModel.SelectedFeature = result.Attributes;
                }
                else
                {
                    result = null;
                    FeatureOverlay.DataContext = null;
                }
            }
            catch (Exception ex)
            {
                FeatureOverlay.DataContext = null;
                System.Windows.MessageBox.Show("HitTest Error: " + ex.Message, "Sample Error");
            }
            finally
            {
                FeatureOverlay.Visibility = (FeatureOverlay.DataContext != null) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion MyMapViewEvents


        #region layersEvents

        private void ButtonAddLayer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Shape files (*.shp)|*.shp";

            if (openFileDialog.ShowDialog() == true)
            {
                OpenShapefileLayer(openFileDialog.FileName);
            }
        }

        private void ButtonRemoveLayer(object sender, RoutedEventArgs e)
        {
            LayerListItem toDelete = mainViewModel.LoadedLayers.First(a => a.FLayer == mainViewModel.SelectedLayer);
            if (toDelete != null)
            {
                mainViewModel.Map.Layers.Remove(toDelete.FLayer);
                mainViewModel.LoadedLayers.Remove(toDelete);
            }
        }

        private void MoveLayerDown(object sender, RoutedEventArgs e)
        {
            Layer selectedLayer = mainViewModel.SelectedLayer;
            if (selectedLayer == null) return;

            int index = mainViewModel.Map.Layers.IndexOf(selectedLayer);
            if (index != mainViewModel.Map.Layers.Count - 1)
                mainViewModel.Map.Layers.Move(index, index + 1);

            LayerListItem LayerListItem = mainViewModel.LoadedLayers.First(lay => lay.FLayer == selectedLayer);
            index = mainViewModel.LoadedLayers.IndexOf(LayerListItem);
            if (index != mainViewModel.LoadedLayers.Count - 1)
                mainViewModel.LoadedLayers.Move(index, index + 1);
        }

        private void MoveLayerUp(object sender, RoutedEventArgs e)
        {
            Layer selectedLayer = mainViewModel.SelectedLayer;
            if (selectedLayer == null) return;

            int index = mainViewModel.Map.Layers.IndexOf(selectedLayer);
            if (index != 0)
                mainViewModel.Map.Layers.Move(index, index - 1);

            LayerListItem LayerListItem = mainViewModel.LoadedLayers.First(lay => lay.FLayer == selectedLayer);
            index = mainViewModel.LoadedLayers.IndexOf(LayerListItem);
            if (index != 0)
                mainViewModel.LoadedLayers.Move(index, index - 1);
        }

        private void LayersChk_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            LayerListItem ch = c.DataContext as LayerListItem;
            Layer l = ch.FLayer;
            MyMapView.Map.Layers[l.ID].IsVisible = c.IsChecked ?? false;
        }

        private void LayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox.SelectedItem == null) return;

            Layer layer = (listBox.SelectedItem as LayerListItem).FLayer;

            if (layer == null) return;

            mainViewModel.SelectedLayer = layer;
            txtInfo.DataContext = mainViewModel.SelectedLayer;

            if (layer is FeatureLayer)
            {
                mainViewModel.LayerProperty = mainViewModel.LoadedLayers.Single(x => x.FLayer == mainViewModel.SelectedLayer).LayerProperty;
            }
        }

        #endregion layersEvents


        #region leftSide

        private void ShowLabels_CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.LayerProperty != null)
            {
                FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
                layer.Labeling.IsEnabled = !layer.Labeling.IsEnabled;
                RefreshMap(layer);
            }
        }

        private void attributesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainViewModel.LayerProperty != null)
            {
                FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
                RefreshMap(layer);
            }
        }

        private void displayStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainViewModel.LayerProperty != null)
            {
                FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
                RefreshMap(layer);
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (mainViewModel.LayerProperty != null)
            {
                FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
                RefreshMap(layer);
            }
        }

        private void MapStyle_ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            var sel = combo.SelectedItem as ComboBoxItem;
            if (sel.Tag == null) { return; }

            // Find and remove the current basemap layer from the map
            if (MyMapView.Map == null) { return; }
            var oldBasemap = mainViewModel.Map.Layers[0];
            if (mainViewModel.Map.Layers["BaseMap"] != null)
                oldBasemap = mainViewModel.Map.Layers["BaseMap"];
            else
                oldBasemap = mainViewModel.Map.Layers[0];
            mainViewModel.Map.Layers.Remove(oldBasemap);

            // Create a new basemap layer
            var newBasemap = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer();

            // Set the ServiceUri with the url defined for the ComboBoxItem's Tag
            newBasemap.ServiceUri = sel.Tag.ToString();

            // Give the layer the same ID so it can still be found with the code above
            newBasemap.ID = "BaseMap";

            // Insert the new basemap layer as the first (bottom) layer in the map
            mainViewModel.Map.Layers.Insert(0, newBasemap);
        }

        #endregion leftSide


        #region rightSideButtons

        private async void SelectRectangle(object sender, RoutedEventArgs e)
        {
            FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
            if (layer == null)
                return;

            await SelectByGeometry(layer, (DrawShape)4);

            RadioButton rbn = sender as RadioButton;
            rbn.IsChecked = false;
        }

        private async void SelectPolygon(object sender, RoutedEventArgs e)
        {
            FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
            if (layer == null)
                return;

            await SelectByGeometry(layer, (DrawShape)2);

            RadioButton rbn = sender as RadioButton;
            rbn.IsChecked = false;
        }

        private async Task SelectByGeometry(FeatureLayer layer, DrawShape shape)
        {
            var rect = await MyMapView.Editor.RequestShapeAsync(shape);

            SpatialQueryFilter filter = new SpatialQueryFilter();
            filter.Geometry = GeometryEngine.Project(rect, layer.FeatureTable.SpatialReference);
            filter.SpatialRelationship = SpatialRelationship.Contains;

            var features = await layer.FeatureTable.QueryAsync(filter);
            var featureIds = features
                .Select(f => Convert.ToInt64(f.Attributes[layer.FeatureTable.ObjectIDField]))
                .ToArray();

            layer.SelectionColor = Colors.Red;
            layer.ClearSelection();
            layer.SelectFeatures(featureIds);
        }

        private void SelectFeature(object sender, RoutedEventArgs e)
        {
            MyMapView.MapViewTapped += MyMapView_MapViewTapped;
        }

        private void Deselect(object sender, RoutedEventArgs e)
        {
            FeatureLayer layer = mainViewModel.SelectedLayer as FeatureLayer;
            if (layer == null)
                return;

            layer.ClearSelection();
        }

        private void AttributeFilterButton(object sender, RoutedEventArgs e)
        {
            if (!(mainViewModel.SelectedLayer is FeatureLayer))
            {
                System.Windows.MessageBox.Show("Select Feature Layer");
                return;
            }

            AttributeViewModel model = new AttributeViewModel();
            model.SourceLayer = mainViewModel.SelectedLayer as FeatureLayer;

            var form = new AttributeFilterWindow(MyMapView);
            form.DataContext = model;
            form.Show();
        }

        private void MeasureDistance(object sender, RoutedEventArgs e)
        {
            RadioButton rbn = sender as RadioButton;
            if (rbn.IsChecked == true)
            {
                if (measureDistance == null)
                    measureDistance = new MeasureDistance(MyMapView, mainViewModel);

                MyMapView.MapViewTapped += measureDistance.MapViewTapped;
            }
            else
            {
                if (measureDistance == null) return;
                MyMapView.MapViewTapped -= measureDistance.MapViewTapped;
                measureDistance.ClearGraphics();
            }
        }

        private void SpatialFilterButton(object sender, RoutedEventArgs e)
        {
            SpatialViewModel model = new SpatialViewModel();
            for (int i = 1; i < mainViewModel.LoadedLayers.Count; i++)
            {
                if (mainViewModel.LoadedLayers[i].FLayer == mainViewModel.SelectedLayer)
                {
                    FeatureLayer Ftest = mainViewModel.LoadedLayers[i].FLayer as FeatureLayer;
                    if (Ftest.SelectedFeatureIDs.Count() != 0)
                        model.SelectedSourceLayer = mainViewModel.LoadedLayers[i];
                }
            }
            if (model.SelectedSourceLayer == null)
                model.Visible = Visibility.Visible;

            model.LoadedLayers = mainViewModel.LoadedLayers
                .Where(item => item.FLayer is FeatureLayer);

            var form = new SpatialFilterWindow(MyMapView);
            form.DataContext = model;
            form.Show();
        }

        private void DeleteOverlaysButton(object sender, RoutedEventArgs e)
        {
            MyMapView.GraphicsOverlays.Clear();
            for (int i = 0; i < mainViewModel.LoadedLayers.Count; i++)
                if (mainViewModel.LoadedLayers[i].Checked)
                {
                    MyMapView.Map.Layers[i].IsVisible = true;
                }
        }

        #endregion rightSideButtons
    }
}

