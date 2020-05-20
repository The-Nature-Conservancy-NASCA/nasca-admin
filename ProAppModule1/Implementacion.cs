using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Implementacion:Element
    {
        public Implementacion(Item item) : base(item)
        {
            index = 8;
        }

        public override object FormatAttributes(Row row)
        {
           var ID_predio = ToString(row, "ID_predio");
           var area_manejo_sostenible = ToDouble(row, "area_manejo_sostenible");
           var area_bosque = ToDouble(row, "area_bosque");
           var areas_p_sostenibles = ToDouble(row, "areas_p_sostenibles");
           var area_restauracion = ToDouble(row, "area_restauracion");
           var momento = ToString(row, "momento");

           var _attributes = new { ID_predio, area_manejo_sostenible, area_bosque, areas_p_sostenibles, area_restauracion, momento };
           return _attributes;
        }
    }
}