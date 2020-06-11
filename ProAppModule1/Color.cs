using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Color : Element
    {
        public Color() : base()
        {
            Index = 12;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Colores coberturas";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var color = ToString(row, "color");
            var cobertura = ToString(row, "cobertura");

            var _attributes = new { color, cobertura };
            return _attributes;
        }
    }
}