using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Region : Element
    {
        public Region(Item item) : base(item)
        {
            index = 0;
        }

        public override object FormatAttributes(Row row)
        {

            var ID_proyecto = ToString(row, "ID_proyecto");
            var ID_region = ToString(row, "ID_region");
            var nombre = ToString(row, "nombre");
            var _attributes = new { ID_proyecto, ID_region, nombre };
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