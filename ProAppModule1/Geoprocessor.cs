using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Internal.Mapping.Symbology;
using ArcGIS.Core.Data;

namespace ProAppModule1
{
    public class Geoprocessor
    {

        private async Task<Table> ConvertExcelToTable(string inputPath, string sheetName, string outPath, string outName)

        {
            var progressDlg = new ProgressDialog("Leyendo datos del archivo Excel seleccionado", "Cancelar", false);
            progressDlg.Show();

            var outTable = System.IO.Path.Combine(outPath, outName);
            var parameters = Geoprocessing.MakeValueArray(inputPath, outTable, sheetName);
            var result = await Geoprocessing.ExecuteToolAsync("conversion.ExcelToTable", parameters, null, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.Default);
            var _outTable = result.Values[0];

            var table = await QueuedTask.Run(() =>
            {
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(outPath)));
                var tbl = geodatabase.OpenDataset<Table>(outName);
                return tbl;
            });

            progressDlg.Hide();
            return table;

        }


    }
}
