using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Textos : Element
    {
        public Textos() : base()
        {
            Index = 15;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Textos Generales";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var identificador = ToString(row, "identificador");
            var texto_espanol = ToString(row, "texto_espanol");
            var texto_ingles = ToString(row, "texto_ingles");

            var _attributes = new { identificador, texto_ingles, texto_espanol };
            return _attributes;
        }
    }
}