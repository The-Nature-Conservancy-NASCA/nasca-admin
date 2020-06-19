using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Textos : Element
    {
        // fields
        //private Proyecto _proyecto;
        private CrearTexto crearElemento = null; // create new element window
        private EditarTexto editarElemento = null; // update element window

        // constructor
        public Textos() : base()
        {
            Index = 15;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Textos generales";
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

            Columns = new List<string> { "OBJECTID", "identificador", "texto_ingles", "texto_espanol" };

            // Parent relationships
            //_proyecto = proyecto;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _identificador; public string identificador { get => _identificador; set { _identificador = value; NotifyPropertyChanged(() => identificador); } }
        private string _texto_ingles; public string texto_ingles { get => _texto_ingles; set { _texto_ingles = value; NotifyPropertyChanged(() => texto_ingles); } }
        private string _texto_espanol; public string texto_espanol { get => _texto_espanol; set { _texto_espanol = value; NotifyPropertyChanged(() => texto_espanol); } }

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
                    identificador = identificador,
                    texto_ingles = texto_ingles,
                    texto_espanol = texto_espanol

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
                    identificador = identificador,
                    texto_ingles = texto_ingles,
                    texto_espanol = texto_espanol
                };
                // -----------------

                return _updateRow;
            }
        }

        // Methods
        public override object FormatAttributes(Row row)
        {

            // Custom fields here

            var identificador = ToString(row, "identificador");
            var texto_espanol = ToString(row, "texto_espanol");
            var texto_ingles = ToString(row, "texto_ingles");
            var _attributes = new { identificador, texto_ingles, texto_espanol };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearTexto { Owner = Application.Current.MainWindow };
                crearElemento.Closed += (o, e) => { crearElemento = null; };

                // Custome parent dependency
                //if (_proyecto.SelectedIndex >= 0)
                //{
                //    var row = _proyecto.data.Rows[_proyecto.SelectedIndex];
                //    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                //}
                //---------------------------

                // Custom default values here
                identificador = "";
                texto_ingles = "";
                texto_espanol = "";
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
                editarElemento = new EditarTexto { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    identificador = Convert.ToString(row["identificador"]);
                    texto_ingles = Convert.ToString(row["texto_ingles"]);
                    texto_espanol = Convert.ToString(row["texto_espanol"]);
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