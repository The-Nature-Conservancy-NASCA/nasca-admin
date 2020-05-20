using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Threading.Tasks;

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
            var ID_cobertura = ToString(row, "ID_cobertura");
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
            var epiteto_espefico = ToString(row, "epiteto_espefico");
            var especie = ToString(row, "especie");
            var autoria = ToString(row, "autoria");
            var nombre_comun = ToString(row, "nombre_comun");
            var sexo = ToString(row, "sexo");
            var momento = ToString(row, "momento");

            var _attributes = new
            {
                ID_region, ID_proyecto, grupo_tnc, cobertura, fecha_identificacion, numero_individuos, institucion, observador, metodo_observacion,
                reino, filo, clase, orden, familia, genero, subgenero, epiteto_espefico, sexo, nombre_comun, especie, autoria, ID_cobertura, momento
            };

            return _attributes;
        }

        public override object Serialize(string json_geom)
        {
            var point = serializer.Deserialize<Point>(json_geom);
            return point;
        }

        public override async Task<Table> ConvertCSVToTable(string inputPath, string outPath, string outName)

        {
            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Override for Biodiversidad has been done");
            var progressDlg = new ProgressDialog("Leyendo datos del archivo CSV seleccionado", "Cancelar", false);
            progressDlg.Show();

            var parameters_totable = Geoprocessing.MakeValueArray(inputPath, outPath, outName);
            var result_totable = await Geoprocessing.ExecuteToolAsync("conversion.TableToTable", parameters_totable, null, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.None);

            var parameters_toxylayer = Geoprocessing.MakeValueArray(inputPath, "longitud", "latitud", "bioxy" + outName);
            var environments_xy = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true, workspace:outPath);
            var result_toxylayer = await Geoprocessing.ExecuteToolAsync("management.MakeXYEventLayer", parameters_toxylayer, environments_xy, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.None);
            var layer = result_toxylayer.Values[0];

            var fc_name = "bio" + outName;
            var parameters_tofc = Geoprocessing.MakeValueArray(layer, outPath, fc_name);
            var environments_tofc = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true, workspace:outPath);
            var result_tofc = await Geoprocessing.ExecuteToolAsync("conversion.FeatureClassToFeatureClass", parameters_tofc, environments_tofc, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.None);



            var table = await QueuedTask.Run(() =>
            {
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(outPath)));
                Table tbl = geodatabase.OpenDataset<FeatureClass>(fc_name);
                return tbl;
            });

            type = "File Geodatabase Feature Class";

            progressDlg.Hide();
            return table;

        }
    }
}