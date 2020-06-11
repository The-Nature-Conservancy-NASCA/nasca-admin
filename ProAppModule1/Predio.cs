using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Predio: Element
    {
        public Predio() : base()
        {
            Index = 1;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Predios";
            ElementType = "el shape file o el feature class de polígonos";
            FilterType = ItemFilters.featureClasses_all;
        }
        public override object FormatAttributes(Row row)
        {
            var ID_proyecto = ToString(row, "ID_proyecto");
            var ID_region = ToString(row, "ID_region");
            var ID_predio = ToString(row, "ID_predio");
            var nombre_predio = ToString(row, "nombre_predio");
            var nombre_propietario = ToString(row, "nombre_propietario");
            var tipo_dominio = ToString(row, "tipo_dominio");
            var departamento = ToString(row, "departamento");
            var municipio = ToString(row, "municipio");
            var vereda = ToString(row ,"vereda");
            var area_ha = ToDouble(row, "area_ha");
            var stok_carbono = ToDouble(row, "stok_carbono");
            var captura_carbono = ToDouble(row, "captura_carbono");

            var _attributes = new
            {
                ID_predio, ID_proyecto, ID_region, nombre_predio, departamento, municipio, vereda, nombre_propietario, tipo_dominio, area_ha, stok_carbono, captura_carbono };

            return _attributes;

        }
        public override object Serialize(string json_geom)
        {
            var rings = serializer.Deserialize<Rings>(json_geom);
            return rings;
        }
    }
}