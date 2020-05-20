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
            var ID_region = ToString(row, "ID_region");
            var ID_proyecto = ToString(row, "ID_proyecto");
            var grupo_tnc = ToString(row, "grupo_tnc");
            var cobertura = ToString(row, "cobertura");
            var fecha_identificacion = ToDate(row, "fecha_identificacion");
            var numero_individuos = ToInt(row, "numero_individuos");
            var institucion = ToString(row, "institucion");
            var observador = ToString(row, "observador");
            var metodo_observacion = ToString(row, "metodo_observacion");
            var reino = ToString(row, "reino");
            var filo = ToString(row, "filo");
            var clase = ToString(row, "clase");
            var orden = ToString(row, "orden");
            var familia = ToString(row, "familia");
            var genero = ToString(row, "genero");
            var subgenero = ToString(row, "subgenero");
            var epiteto = ToString(row, "epiteto");
            var sexo = ToString(row, "sexo");
            var nombre_comun = ToString(row, "nombre_comun");
            var especie = ToString(row, "especie");
            var autoria = ToString(row, "autoria");
            var ID_cobertura = ToString(row, "ID_cobertura");
            var momento = ToString(row, "momento");

            var _attributes = new
            {
                ID_region, ID_proyecto, grupo_tnc, cobertura, fecha_identificacion, numero_individuos, institucion, observador, metodo_observacion,
                reino, filo, clase, orden, familia, genero, subgenero, epiteto, sexo, nombre_comun, especie, autoria, ID_cobertura, momento
            };

            return _attributes;
        }

        public override object Serialize(string json_geom)
        {
            var point = serializer.Deserialize<Point>(json_geom);
            return point;
        }
    }
}