using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Proyecto : Element
    {
        public Proyecto(Item item) : base(item)
        {
            index = 4;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_estrategia = ToString(row, "ID_estrategia");
            var ID_proyecto = ToString(row, "ID_proyecto");
            var nombre = ToString(row, "nombre");
            var descripcion = ToString(row, "descripcion");
            var descripcion_carbono = ToString(row, "descripcion_carbono");
            var descripcion_coberturas = ToString(row, "descripcion_coberturas");
            var descripcion_biodiversidad = ToString(row, "descripcion_biodiversidad");
            var descripcion_implementaciones = ToString(row, "descripcion_implementaciones");
            var descripcion_aliados = ToString(row, "descripcion_aliados");
            var descripcion_contribuciones = ToString(row, "descripcion_contribuciones");
            var descripcion_metas = ToString(row, "descripcion_metas");
            var descripcion_participantes = ToString(row, "descripcion_participantes");
            var color = ToString(row, "color");
            var fondo = ToString(row, "fondo");
            var icono = ToString(row, "icono");
            var fecha_linea_base = ToDate(row, "fecha_linea_base");
            var fecha_seguimiento1 = ToDate(row, "fecha_seguimiento1");
            var fecha_seguimiento2 = ToDate(row, "fecha_seguimiento2");
            var fecha_cierre = ToDate(row, "fecha_cierre");
            var cierre = ToInt(row, "cierre");

            var _attributes = new
            {
                ID_estrategia, ID_proyecto, nombre, descripcion, descripcion_carbono, descripcion_coberturas, descripcion_biodiversidad, descripcion_implementaciones,
                descripcion_aliados, descripcion_contribuciones, descripcion_metas, descripcion_participantes, color, fondo, icono, fecha_linea_base, fecha_seguimiento1,
                fecha_seguimiento2, fecha_cierre, cierre

            };
            return _attributes;
        }
    }
}