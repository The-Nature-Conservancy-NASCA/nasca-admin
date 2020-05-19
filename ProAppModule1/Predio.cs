using System;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Predio: Element
    {
        public Predio(Item item) : base(item)
        {
            index = 1;
        }
        public override object FormatAttributes(Row row)
        {
            var ID_predio = ToString(row, "ID_predio");
            var ID_proyecto = ToString(row, "ID_proyect");
            var ID_region = ToString(row, "ID_region");
            var nombre = ToString(row, "nombre_pre");
            var departamento = ToString(row, "departamen");
            var municipio = ToString(row, "municipio");
            var vereda = ToString(row ,"vereda");
            var nombre_propietario = ToString(row, "nombre_pro");
            var tipo_dominio = ToString(row, "tipo_domin");
            var AreaHa = ToDouble(row, "AreaHa");
            var stok_carbono = ToDouble(row, "stock_carb");
            var captura_carbono = ToDouble(row, "captura_ca");

            Object _attributes = new
            {
                ID_predio, ID_proyecto, ID_region, nombre, departamento, municipio, vereda, nombre_propietario, tipo_dominio, AreaHa, stok_carbono, captura_carbono

            };
            return _attributes;

        }
        public override object Serialize(string json_geom)
        {
            Rings rings;
            rings = serializer.Deserialize<Rings>(json_geom);
            return rings;
        }
    }
}