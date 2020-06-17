using System;
using System.Collections.Generic;
using System.Linq;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using System.Data;
using System.Windows.Input;
using System.Windows;

namespace ProAppModule1
{
    /// View Model for the dockpane
    public class Dockpane1ViewModel : DockPane
    {
        // Dockpane id
        private const string _dockPaneID = "ProAppModule1_Dockpane1";

        // Constants
        private const string _serviceURL = "https://services.arcgis.com/F7DSX1DSNSiWmOqh/arcgis/rest/services/GeodatabaseTNC/FeatureServer"; // Production
        //private const string _serviceURL = "https://services.arcgis.com/F7DSX1DSNSiWmOqh/arcgis/rest/services/GeodatabaseTNC_Pruebas/FeatureServer"; // Testing


        public Region Region { get; } = new Region();
        public Predio Predio { get; } = new Predio();
        public Biodiversidad Biodiversidad { get; } = new Biodiversidad();
        public Estrategia Estrategia { get; set; } 
        public Proyecto Proyecto { get; set; } 
        public Participante Participante { get; } = new Participante();
        public Meta Meta { get; } = new Meta();
        public Aliado Aliado { get; } = new Aliado();
        public Implementacion Implementacion { get; } = new Implementacion();
        public Contribucion Contribucion { get; } = new Contribucion();
        public Carrusel Carrusel { get; } = new Carrusel();
        public Carbono Carbono { get; } = new Carbono();
        public Color Color { get; } = new Color();
        public Cobertura Cobertura { get; } = new Cobertura();
        public IconosBiodiversidad IconosBiodiversidad { get; } = new IconosBiodiversidad();
        public Textos Textos { get; } = new Textos();

        // Dock Panel Constructor 
        protected Dockpane1ViewModel()
        {
            Estrategia = new Estrategia();
            Proyecto =  new Proyecto(Estrategia);
            Estrategia.Proyecto = Proyecto;

            // Load data 
            if (token != "")
            {
                Estrategia.LoadData();
                Proyecto.LoadData();
            }

        }

        internal static string token
        {
            get
            {
                var active_portal = ArcGISPortalManager.Current.GetActivePortal();
                return active_portal.GetToken(); ;
            }
        }

        // Show the DockPane.
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        // Show the DockPane.
        internal static void Hide()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            pane.Hide();

        }


        public void NotAvailable()
        {
            MessageBox.Show("Servicio web no disponible");
        }


    }

    /// Button implementation to show the DockPane.
    internal class Dockpane1_ShowButton : Button
    {
        protected override void OnClick()
        {
            if (Dockpane1ViewModel.token != "")
            {
                //Dockpane1View.ReinitializeComponent();
                //Module1.Reinitialice();
                Dockpane1ViewModel.Show();
            }

            else
            {
                MessageBox.Show("Debe iniciar sesión en ArcGIS Pro para acceder al administrador");

            }

        }
    }
}
