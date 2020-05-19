using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Estrategia:Element
    {

        public Estrategia(Item item) : base(item)
        {
            index = 3;
        }

        public override object FormatAttributes(Row row)
        {

            var ID_estrategia = ToString(row, "ID_estrategia");
            var nombre = ToString(row, "nombre");
            var color = ToString(row, "color");
            var fondo = ToString(row, "fondo");
            var icono = ToString(row, "icono");
            var descripcion = ToString(row, "descripcion");

            var _attributes = new { ID_estrategia, nombre, color, fondo, icono, descripcion };
            return _attributes;

        }

    }
}