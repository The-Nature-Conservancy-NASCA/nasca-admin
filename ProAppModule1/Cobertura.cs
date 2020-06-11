using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Cobertura:Element
    {
        public Cobertura() : base()
        {
            Index = 13;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Cobertura";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_predio = ToString(row, "ID_predio");
            var corine1 = ToString(row, "corine1");
            var corine2 = ToString(row, "corine2");
            var corine3 = ToString(row, "corine3");
            var cobertura_comun = ToString(row, "cobertura_comun");
            var cobertura_proyecto = ToString(row, "cobertura_proyecto");
            var subcobertura_proyecto = ToString(row, "subcobertura_proyecto");
            var uso = ToString(row, "uso");
            var verificacion = ToString(row, "verificacion");
            var fecha_establecimiento = ToDate(row, "fecha_establecimiento");
            var fecha_visita = ToDate(row, "fecha_visita");
            var edad = ToInt(row, "edad");
            var area = ToDouble(row, "area");
            var momento = ToString(row, "momento");

            var _attributes = new { ID_predio, corine1, corine2, corine3, cobertura_comun, cobertura_proyecto, uso, verificacion, fecha_establecimiento ,edad, fecha_visita, subcobertura_proyecto, area, momento };
            return _attributes;

        }
    }
}