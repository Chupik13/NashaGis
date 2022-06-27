using SGP.Gis;
using SGP.Gis.Attributes;
using SGP.Gis.Carto.Layers;
using SGP.Gis.Catalog;
using SGP.Gis.Commands;
using SGP.Gis.Controls;
using SGP.Gis.CoordinateSystem;
using SGP.Gis.Features;
using SGP.Gis.Features.Visitors;
using SGP.Gis.Filters;
using SGP.Gis.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace NashaGis2
{
    [GisApplication(GisApplicationType.Map)]
    [Guid("1EAE5EAA-567D-448A-8CD8-B17AE28C554B")]
    [BarItem(true, 5, CommandIconSize.Large, true)]
    public class SampleLargeButton : ButtonCommandInfo
    {
        public SampleLargeButton() : base("Тестовое задание", "Тестовое задание", "Выборка", SGP.Gis.Resources.Icons.IconType.Run)
        {
            IsEnabled = true;
            IsVisible = true;
        }

        public override void Execute(object parameter)
        {
            var mapControl = Application.Context.Resolve<IMapControl>();
            var map = mapControl.Document;

            var layers = map.Layers.ToList();
            if (layers.Count == 0)
            {
                MessageBox.Show("Слои отсутствуют");
            }
            else
            {
                var model = new SelectFeatureLayerModel
                {
                    Map = mapControl
                };
                var layer = model.FeatureLayers.FirstOrDefault();
                var queryFilter = new QueryFilter();
                queryFilter.WhereClause = "(Недропольз)='АО ОРЕНБУРГНЕФТЬ'";
                var result = layer.Search(queryFilter)
                    .ToEnumerable().Where(x => x.GetValue("Название") != null)
                    .MaxBy(x => x.Shape.GetArea(x => x));
                MessageBox.Show($"Название: {result.GetValue("Название").ToString()} " +
                    $"Площадь: {result.Shape.GetArea(x=>x)}");
            }
        }
    }
    
    public class SelectFeatureLayerModel
    {
        public IMapControl Map { get; set; }
        public IFeatureLayer[] FeatureLayers
        {
            get
            {
                return Map?.Document.GetAllLayers().OfType<IFeatureLayer>().ToArray();
            }
        }
    }
}



