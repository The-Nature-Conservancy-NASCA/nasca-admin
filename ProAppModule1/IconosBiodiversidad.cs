using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class IconosBiodiversidad : Element
    {
        public IconosBiodiversidad() : base()
        {
            Index = 14;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Iconos Biodiversidad";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var grupo_tnc = ToString(row, "grupo_tnc");
            var url = ToString(row, "url");

            var _attributes = new { grupo_tnc, url };
            return _attributes;
        }
    }
}