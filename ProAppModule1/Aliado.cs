using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Aliado : Element
    {
        // fields
        private Proyecto _proyecto;
        private CrearAliado crearElemento = null; // create new element window
        private EditarAliado editarElemento = null; // update element window

        // constructor
        public Aliado(Proyecto proyecto) : base()
        {
            Index = 7;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Aliado";
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

            Columns = new List<string> { "OBJECTID", "ID_proyecto", "tipo", "nombre", "logo" };
                                        
            // Parent relationships
            _proyecto = proyecto;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _ID_proyecto; public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; NotifyPropertyChanged(() => ID_proyecto); } }
        private string _tipo; public string tipo { get => _tipo; set { _tipo = value; NotifyPropertyChanged(() => tipo); } }
        private string _nombre; public string nombre { get => _nombre; set { _nombre = value; NotifyPropertyChanged(() => nombre); } }
        private string _logo; public string logo { get => _logo; set { _logo = value; NotifyPropertyChanged(() => logo); } }

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
                    ID_proyecto = ID_proyecto,
                    tipo = tipo,
                    nombre = nombre,
                    logo = logo
                
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
                    ID_proyecto = ID_proyecto,
                    tipo = tipo,
                    nombre = nombre,
                    logo = logo
                };
                // -----------------

                return _updateRow;
            }
        }

        // Methods
        public override object FormatAttributes(Row row)
        {

            // Custom fields here
            var ID_proyecto = ToString(row, "ID_proyecto");
            var Tipo = ToString(row, "Tipo");
            var nombre = ToString(row, "nombre");
            var logo = ToString(row, "logo");
            var url = ToString(row, "url");

            var _attributes = new { ID_proyecto, Tipo, nombre, logo, url };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearAliado { Owner = Application.Current.MainWindow };
                crearElemento.Closed += (o, e) => { crearElemento = null; };

                // Custome parent dependency
                //if (_proyecto.SelectedIndex >= 0)
                //{
                //    var row = _proyecto.data.Rows[_proyecto.SelectedIndex];
                //    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                //}
                //---------------------------

                // Custom default values here
                ID_proyecto = "";
                tipo = "";
                nombre = "";
                logo = "";
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
                editarElemento = new EditarAliado { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                    tipo = Convert.ToString(row["tipo"]);
                    nombre = Convert.ToString(row["nombre"]);
                    logo = Convert.ToString(row["logo"]);
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