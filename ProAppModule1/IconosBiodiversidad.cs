using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class IconosBiodiversidad : Element
    {
        // fields
        //private Proyecto _proyecto;
        private CrearIcono crearElemento = null; // create new element window
        private EditarIcono editarElemento = null; // update element window

        // constructor
        public IconosBiodiversidad() : base()
        {
            Index = 14;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Iconos biodiversidad";
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

            Columns = new List<string> { "OBJECTID", "grupo_tnc", "url" };

            // Parent relationships
            //_proyecto = proyecto;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _grupo_tnc; public string grupo_tnc { get => _grupo_tnc; set { _grupo_tnc = value; NotifyPropertyChanged(() => grupo_tnc); } }
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
                    grupo_tnc = grupo_tnc,
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
                    grupo_tnc = grupo_tnc,
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
            var grupo_tnc = ToString(row, "grupo_tnc");
            var url = ToString(row, "url");


            var _attributes = new { grupo_tnc, url };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearIcono { Owner = Application.Current.MainWindow };
                crearElemento.Closed += (o, e) => { crearElemento = null; };

                // Custome parent dependency
                //if (_proyecto.SelectedIndex >= 0)
                //{
                //    var row = _proyecto.data.Rows[_proyecto.SelectedIndex];
                //    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                //}
                //---------------------------

                // Custom default values here
                grupo_tnc = "";
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
                editarElemento = new EditarIcono { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    grupo_tnc = Convert.ToString(row["grupo_tnc"]);
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