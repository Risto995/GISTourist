using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace TouristGIS.Filters
{
    public class AttributeFilter
    {
        public async static Task<IEnumerable<Feature>> DoAttributeFilter(FeatureLayer layer, string query)
        {
            try
            {
                QueryFilter filter = new QueryFilter() { WhereClause = query };
                var result = await layer.FeatureTable.QueryAsync(filter);
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
