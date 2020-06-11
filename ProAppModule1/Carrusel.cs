using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Carrusel:Element
    {
        public Carrusel() : base()
        {
            Index = 10;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Imagenes Carrusel";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_region = ToString(row, "ID_Region");
            var Especie = ToString(row, "Especie");
            var nombre_comun = ToString(row, "nombre_comun");
            var URL = ToString(row, "URL");

            var _attributes = new { ID_region, Especie, nombre_comun, URL };
            return _attributes;
        }
    }
}