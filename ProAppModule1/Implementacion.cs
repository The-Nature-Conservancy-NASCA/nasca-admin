using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Implementacion:Element
    {
        public Implementacion() : base()
        {
            Index = 8;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Implementación";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
           var ID_predio = ToString(row, "ID_predio");
           var area_manejo_sostenible = ToDouble(row, "area_manejo_sostenible");
           var area_bosque = ToDouble(row, "area_bosque");
           var area_sistemas_productivos_sostenibles = ToDouble(row, "area_sistemas_productivos_sostenibles");
           var area_restauracion = ToDouble(row, "area_restauracion");
           var momento = ToString(row, "momento");

           var _attributes = new { ID_predio, area_manejo_sostenible, area_bosque, area_sistemas_productivos_sostenibles, area_restauracion, momento };
           return _attributes;
        }
    }
}