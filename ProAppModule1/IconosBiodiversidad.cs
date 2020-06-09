using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class IconosBiodiversidad : Element
    {
        public IconosBiodiversidad(Item item) : base(item)
        {
            index = 14;
        }

        public override object FormatAttributes(Row row)
        {
            var grupo_tnc = ToString(row, "grupo_tnc");
            var url = ToString(row, "url");

            var _attributes = new { grupo_tnc, url };
            return _attributes;
        }
    }
}