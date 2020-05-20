using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Carrusel:Element
    {
        public Carrusel(Item item) : base(item)
        {
            index = 10;
        }

        public override object FormatAttributes(Row row)
        {
            var Especie = ToString(row, "Especie");
            var nombre_comun = ToString(row, "nombre_comun");
            var URL = ToString(row, "URL");

            var _attributes = new { Especie, nombre_comun, URL };
            return _attributes;
        }
    }
}