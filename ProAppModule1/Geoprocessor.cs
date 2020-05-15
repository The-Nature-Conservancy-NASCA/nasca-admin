using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;

namespace ProAppModule1
{
    public class Geoprocessor
    {

        public async Task<Uri> ConvertToShapefile(Item item) {

            var progressDlg = new ProgressDialog("Leyendo datos del Feature class seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            var conversionProcess = await QueuedTask.Run(() => {

                string outPath = Project.Current.HomeFolderPath;
                //string outPath = "in_memory";
                string toolPath = "conversion.FeatureClassToShapefile";
                var parameters = Geoprocessing.MakeValueArray(item.Path, outPath);
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);
                var result = Geoprocessing.ExecuteToolAsync(toolPath, parameters, environments, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.Default);
                return result;
            });

            Uri shp_path = new System.Uri(conversionProcess.Values[0]);

            progressDlg.Hide();
            return shp_path;


        }

        public async Task<uint> GetCount(Uri path) {

            var progressDlg = new ProgressDialog("Contando número de registros", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            var countProcess= await QueuedTask.Run(() => {
                string toolPath = "management.GetCount";
                var parameters = Geoprocessing.MakeValueArray(path);
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);
                var result = Geoprocessing.ExecuteToolAsync(toolPath, parameters, environments, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.Default);
                return result;
            });

            var count = Convert.ToUInt32(countProcess.Values[0]);

            progressDlg.Hide();
            return count;

        }

    }
}
