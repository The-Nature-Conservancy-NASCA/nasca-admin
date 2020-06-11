using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Participante : Element
    {
        public Participante() : base()
        {
            Index = 5;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Participante";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_proyecto = ToString(row, "ID_proyecto");
            var numero_hombres = ToInt(row, "numero_hombres");
            var numero_mujeres = ToInt(row, "numero_mujeres");
            var numero_indigenas = ToInt(row, "numero_indigenas");
            var numero_campesinos = ToInt(row, "numero_campesinos");
            var numero_sin_informacion = ToInt(row,  "numero_sin_informacion");
            var momento = ToInt(row, "momento");

            var _attributes = new { ID_proyecto, numero_hombres, numero_mujeres, numero_indigenas, numero_campesinos, numero_sin_informacion, momento };
            return _attributes;
        }
    }
}