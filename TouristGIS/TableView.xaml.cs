using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Esri.ArcGISRuntime.Data;

namespace TouristGIS
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : Window
    {
        private IEnumerable<Feature> enumerable;

        public TableView()
        {
            InitializeComponent();
        }

        public TableView(IEnumerable<Feature> enumerable)
        {
            this.enumerable = enumerable;

            InitializeComponent();

            if (enumerable == null)
                return;

            var first = enumerable.FirstOrDefault();
            if (first == null)
                return;

            var gridView = new GridView();
            foreach (var attr in first.Attributes)
            {
                gridView.Columns.Add(
                    new GridViewColumn()
                    {
                        Header = attr.Key,
                        DisplayMemberBinding = new Binding("Attributes[" + attr.Key + "]"),
                        Width = 100
                    });
            }
            resultsGrid.View = gridView;

            resultsGrid.ItemsSource = enumerable;

        }
    }
}
