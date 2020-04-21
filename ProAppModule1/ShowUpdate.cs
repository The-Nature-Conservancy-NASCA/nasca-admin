using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace ProAppModule1
{
    internal class ShowUpdate : Button
    {

        private Update _update = null;

        protected override void OnClick()
        {
            //already open?
            if (_update != null)
                return;
            _update = new Update();
            _update.Owner = FrameworkApplication.Current.MainWindow;
            _update.Closed += (o, e) => { _update = null; };
            _update.Show();
            //uncomment for modal
            //_update.ShowDialog();
        }

    }
}
