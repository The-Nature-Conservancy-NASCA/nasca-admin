using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Region : Element
    {
        public Region() : base()
        {
            Index = 0;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Región";
            ElementType = "el shape file o el feature class de polígonos";
            FilterType = ItemFilters.featureClasses_all;
        }

        public override object FormatAttributes(Row row)
        {

            var ID_proyecto = ToString(row, "ID_proyecto");
            var ID_region = ToString(row, "ID_region");
            var nombre = ToString(row, "nombre");
            var fecha = ToDate(row, "fecha");
            var _attributes = new { ID_proyecto, ID_region, nombre, fecha };
            return _attributes;

        }

        public override object Serialize(string json_geom)
        {
            var rings = serializer.Deserialize<Rings>(json_geom);
            return rings;
        }
    }
}