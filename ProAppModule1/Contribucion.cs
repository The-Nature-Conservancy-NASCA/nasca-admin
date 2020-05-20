using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Contribucion : Element
    {
        public Contribucion(Item item) : base(item)
        {
            index = 9;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_proyecto = ToString(row, "ID_proyecto");
            var tipo = ToString(row, "tipo");
            var nombre = ToString(row, "nombre");
            var logo = ToString(row, "logo");
            var url = ToString(row, "url");

            var _attributes = new { ID_proyecto, tipo, nombre, logo, url };
            return _attributes;

        }
    }
}