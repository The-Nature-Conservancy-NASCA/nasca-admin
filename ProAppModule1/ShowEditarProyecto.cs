using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace ProAppModule1
{
    internal class ShowEditarProyecto : Button
    {

        private EditarProyecto _editarproyecto = null;

        protected override void OnClick()
        {
            //already open?
            if (_editarproyecto != null)
                return;
            _editarproyecto = new EditarProyecto();
            _editarproyecto.Owner = FrameworkApplication.Current.MainWindow;
            _editarproyecto.Closed += (o, e) => { _editarproyecto = null; };
            _editarproyecto.Show();
            //uncomment for modal
            //_editarproyecto.ShowDialog();
        }

    }
}
