using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public class Carbono : Element
    {
        public Carbono(Item item) : base(item)
        {
            index = 11;
        }

        public override object FormatAttributes(Row row)
        {
            var ID_region = ToString(row, "ID_region");
            var cobertura = ToString(row, "cobertura");
            var compartimiento = ToString(row, "compartimiento");
            var sub_compartimiento = ToString(row, "sub_compartimiento");
            var SNC = ToString(row, "SNC");
            var T1 = ToDouble(row, "T1");
            var T2 = ToDouble(row, "T2");
            var T3 = ToDouble(row,"T3");
            var T4 = ToDouble(row,"T4");
            var T5 = ToDouble(row,"T5");
            var T6 = ToDouble(row,"T6");
            var T7 = ToDouble(row,"T7");
            var T8 = ToDouble(row,"T8");
            var T9 = ToDouble(row,"T9");
            var T10 = ToDouble(row,"T10");
            var T11 = ToDouble(row,"T11");
            var T12 = ToDouble(row,"T12");
            var T13 = ToDouble(row,"T13");
            var T14 = ToDouble(row,"T14");
            var T15 = ToDouble(row,"T15");
            var T16 = ToDouble(row,"T16");
            var T17 = ToDouble(row,"T17");
            var T18 = ToDouble(row,"T18");
            var T19 = ToDouble(row,"T19");
            var T20 = ToDouble(row,"T20");

            var _attributes = new
            {
                cobertura, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, ID_region,
                SNC, compartimiento, sub_compartimiento

            };
            return _attributes;
        }
    }
}