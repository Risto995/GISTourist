using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TouristGIS.ViewModels;

namespace TouristGIS.Filters
{
    public class SpatialFilter
    {
        public async Task<IEnumerable<Feature>> DoSpatialFilter(FeatureLayer sourceLayer, FeatureLayer destinationLayer, int radius, SpatialRelationship relation, string sourceQuery, string destinationQuery, bool inRadius)
        {
            IEnumerable<Feature> sourceFeatures;
            if (sourceLayer.SelectedFeatureIDs.Count() == 0)
                sourceFeatures = await sourceLayer.FeatureTable.QueryAsync(new QueryFilter() { WhereClause = sourceQuery });
            else sourceFeatures = await sourceLayer.FeatureTable.QueryAsync(sourceLayer.SelectedFeatureIDs);

            if (sourceFeatures.Count() > 1)
            {
                if (inRadius)
                    return await DoDistanceSpatial(sourceFeatures, destinationLayer, radius, destinationQuery);
                else return await DoRelationSpatial(sourceFeatures, destinationLayer, relation, destinationQuery);
            }

            List<Feature> returnList = new List<Feature>();

            foreach (var feature in sourceFeatures)
            {
                SpatialQueryFilter filter = new SpatialQueryFilter();

                if (inRadius)
                {
                    filter.Geometry = GeometryEngine.Buffer(feature.Geometry, radius);
                    filter.SpatialRelationship = SpatialRelationship.Within;
                }
                else
                {
                    filter.Geometry = feature.Geometry;
                    filter.SpatialRelationship = relation;
                }
                filter.WhereClause = destinationQuery;

                var result = await destinationLayer.FeatureTable.QueryAsync(filter);
                List<Feature> resultList = result.ToList();
                returnList.AddRange(resultList);
            }

            return returnList.Distinct().ToList();
        }

        public async Task<IEnumerable<Feature>> DoDistanceSpatial(IEnumerable<Feature> sourceFeatures, FeatureLayer destinationLayer, int radius, string destinationQuery)
        {
            List<Feature> returnList = new List<Feature>();
            IEnumerable<Feature> destinationFeatures = await destinationLayer.FeatureTable.QueryAsync(new QueryFilter() { WhereClause = destinationQuery });
            foreach (var sourceFeature in sourceFeatures)
                foreach (var destinationFeature in destinationFeatures)
                    if (GeometryEngine.Distance(sourceFeature.Geometry, destinationFeature.Geometry) < radius)
                        returnList.Add(destinationFeature);
            return returnList.Distinct().ToList();
        }

        public async Task<IEnumerable<Feature>> DoRelationSpatial(IEnumerable<Feature> sourceFeatures, FeatureLayer destinationLayer, SpatialRelationship relation, string destinationQuery)
        {
            List<Feature> returnList = new List<Feature>();
            IEnumerable<Feature> destinationFeatures = await destinationLayer.FeatureTable.QueryAsync(new QueryFilter() { WhereClause = destinationQuery });
            foreach (var sourceFeature in sourceFeatures)
                foreach (var destinationFeature in destinationFeatures)
                {
                    switch (relation)
                    {
                        case SpatialRelationship.Contains:
                            if (GeometryEngine.Contains(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        case SpatialRelationship.Crosses:
                            if (GeometryEngine.Crosses(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        case SpatialRelationship.Intersects:
                            if (GeometryEngine.Intersects(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        case SpatialRelationship.Overlaps:
                            if (GeometryEngine.Overlaps(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        case SpatialRelationship.Touches:
                            if (GeometryEngine.Touches(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        case SpatialRelationship.Within:
                            if (GeometryEngine.Within(destinationFeature.Geometry, sourceFeature.Geometry))
                                returnList.Add(destinationFeature);
                            break;
                        default: break;
                    }
                }
            return returnList.Distinct().ToList();
        }
    }
}

