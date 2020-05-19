using System;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Biodiversidad : Element
    {
        public Biodiversidad(Item item) : base(item)
        {
            index = 2;
        }

        public override object FormatAttributes(Row row)
        {
            string ID_region = ToString(row, "ID_region");
            string ID_proyecto = ToString(row, "ID_proyecto");
            string grupo_tnc = ToString(row, "grupo_tnc");
            string cobertura = ToString(row, "cobertura");
            double fecha_identificacion = ToDouble(row, "fecha_identificacion");
            int numero_individuos = ToInt(row, "numero_individuos");
            string institucion = ToString(row, "institucion");
            string observador = ToString(row, "observador");
            string metodo_observacion = ToString(row, "metodo_observacion");
            string reino = ToString(row, "reino");
            string filo = ToString(row, "filo");
            string clase = ToString(row, "clase");
            string orden = ToString(row, "orden");
            string familia = ToString(row, "familia");
            string genero = ToString(row, "genero");
            string subgenero = ToString(row, "subgenero");
            string epiteto = ToString(row, "epiteto");
            string sexo = ToString(row, "sexo");
            string nombre_comun = ToString(row, "nombre_comun");
            string especie = ToString(row, "especie");
            string autoria = ToString(row, "autoria");
            string ID_cobertura = ToString(row, "ID_cobertura");
            string momento = ToString(row, "momento");

            Object _attributes = new {ID_region, especie, fecha_identificacion, numero_individuos};

            return _attributes;

        }

        public override object Serialize(string json_geom)
        {
            Point point;
            point = serializer.Deserialize<Point>(json_geom);
            return point;
        }

    }
}