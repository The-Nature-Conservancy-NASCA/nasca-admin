using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Color : Element
    {
        public Color(Item item) : base(item)
        {
            index = 12;
        }

        public override object FormatAttributes(Row row)
        {
            var color = ToString(row, "color");
            var cobertura = ToString(row, "cobertura");

            var _attributes = new { color, cobertura };
            return _attributes;
        }
    }
}