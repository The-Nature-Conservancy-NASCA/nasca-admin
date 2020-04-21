using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace ProAppModule1
{
    internal class ShowCrearProyecto : Button
    {

        private CrearProyecto _crearproyecto = null;

        protected override void OnClick()
        {
            //already open?
            if (_crearproyecto != null)
                return;
            _crearproyecto = new CrearProyecto();
            _crearproyecto.Owner = FrameworkApplication.Current.MainWindow;
            _crearproyecto.Closed += (o, e) => { _crearproyecto = null; };
            _crearproyecto.Show();
            //uncomment for modal
            //_crearproyecto.ShowDialog();
        }

    }
}
