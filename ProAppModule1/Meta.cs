using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Meta : Element
    {
        public Meta(Item item) : base(item)
        {
            index = 6;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_proyecto = ToString(row, "ID_proyecto");
            var meta = ToString(row, "meta");
            var valor = ToDouble(row, "valor");
            var progreso = ToDouble(row, "progreso");
            var unidad = ToString(row, "unidad");
            var momento = ToString(row, "momento");

            var _attributes = new { ID_proyecto, meta, valor, progreso, unidad, momento };
            return _attributes;
        }
    }
}