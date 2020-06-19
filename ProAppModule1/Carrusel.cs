using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Carrusel : Element
    {
        // fields
        private Region _region;
        private CrearImagen crearElemento = null; // create new element window
        private EditarImagen editarElemento = null; // update element window

        // constructor
        public Carrusel(Region region) : base()
        {
            Index = 10;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Imágenes carrusel";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
            Service = $"{serviceURL}/{Index}";

            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);
            AddNewRowCommand = new RelayCommand(() => { AddNewRow(AddRow); crearElemento?.Close(); }, () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => { UpdateSelectedRow(Objectid, UpdateRow); editarElemento?.Close(); }, () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);


            // Relationship commands 
            //SelectionCommand = new RelayCommand(() => _proyecto.OnElementoSelecionado(), () => true);

            Columns = new List<string> { "OBJECTID", "ID_region", "Especie", "nombre_comun", "URL" };

            // Parent relationships
            _region = region;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _ID_region; public string ID_region { get => _ID_region; set { _ID_region = value; NotifyPropertyChanged(() => ID_region); } }
        private string _especie; public string especie { get => _especie; set { _especie = value; NotifyPropertyChanged(() => especie); } }
        private string _nombre_comun; public string nombre_comun { get => _nombre_comun; set { _nombre_comun = value; NotifyPropertyChanged(() => nombre_comun); } }
        private string _url; public string url { get => _url; set { _url = value; NotifyPropertyChanged(() => url); } }

        // Commands
        public ICommand SelectionCommand { get; }
        public ICommand ShowProWindow { get; }
        public ICommand ShowProWindowUpdateCommand { get; }
        public ICommand UnselectRowCommand { get; }
        public ICommand AddNewRowCommand { get; }
        public ICommand UpdateSelectedRowCommand { get; }
        public ICommand EliminateSelectedRow { get; }


        // Advanced properties
        public override object AddRow
        {
            get
            {
                // Custom fields here
                var _addRow = new
                {
                    ID_region = ID_region,
                    especie = especie,
                    nombre_comun = nombre_comun,
                    url = url

                };
                // ----------------

                return _addRow;
            }
        }
        public override object UpdateRow
        {
            get
            {
                object _updateRow = new { };
                if (Objectid < 0)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de {ElementName}", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    return _updateRow;
                }

                // Custom fields Here
                _updateRow = new
                {
                    OBJECTID = Objectid,
                    ID_region = ID_region,
                    especie = especie,
                    nombre_comun = nombre_comun,
                    url = url
                };
                // -----------------

                return _updateRow;
            }
        }

        // Methods
        public override object FormatAttributes(Row row)
        {

            // Custom fields here
            var ID_region = ToString(row, "ID_Region");
            var Especie = ToString(row, "Especie");
            var nombre_comun = ToString(row, "nombre_comun");
            var URL = ToString(row, "URL");

            var _attributes = new { ID_region, especie, nombre_comun, url };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearImagen { Owner = Application.Current.MainWindow };
                crearElemento.Closed += (o, e) => { crearElemento = null; };

                // Custome parent dependency
                //if (_proyecto.SelectedIndex >= 0)
                //{
                //    var row = _proyecto.data.Rows[_proyecto.SelectedIndex];
                //    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                //}
                //---------------------------

                // Custom default values here
                ID_region = "";
                especie = "";
                nombre_comun = "";
                url = "";
                // Custom default values here

                crearElemento.DataContext = this;
                crearElemento.Show();

            }

        }

        // Method to show the Pro Window
        public void ShowProWindowUpdate()

        {
            {
                //already open?
                if (editarElemento != null)
                    return;
                editarElemento = new EditarImagen { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    ID_region = Convert.ToString(row["ID_region"]);
                    especie = Convert.ToString(row["especie"]);
                    nombre_comun = Convert.ToString(row["nombre_comun"]);
                    url = Convert.ToString(row["url"]);
                }
                else
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de {ElementName}", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    editarElemento.Close();
                    return;
                }

                editarElemento.DataContext = this;
                editarElemento.Show();
            }
        }

        public override async void UnselectRow()
        {
            SelectedIndex = -1;

            // Custom child dependency
            // custom child dependency here (view Estrategia)
        }


        public async void EliminateRow()
        {
            if (SelectedIndex < 0) return;
            var answer = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("¿Desea eliminar el elemento seleccionado y sus registros relacionados?",
                "Borrar registro", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (answer != MessageBoxResult.Yes) return;
            if (Objectid < 0) return;
            WebInteraction.DeleteFeatures(Service, Objectid);
            await FillDataTable();
            NotifyPropertyChanged(() => data);
        }

    }
}