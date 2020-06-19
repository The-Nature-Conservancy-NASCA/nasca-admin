using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Meta : Element
    {
        // fields
        private Proyecto _proyecto;
        private CrearMeta crearElemento = null; // create new element window
        private EditarMeta editarElemento = null; // update element window

        // constructor
        public Meta(Proyecto proyecto) : base()
        {
            Index = 6;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Meta";
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

            Columns = new List<string> { "OBJECTID", "ID_proyecto", "meta", "valor", "progreso", "unidad", "momento" };

            // Parent relationships
            _proyecto = proyecto;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _ID_proyecto; public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; NotifyPropertyChanged(() => ID_proyecto); } }
        private string _meta; public string meta { get => _meta; set { _meta = value; NotifyPropertyChanged(() => meta); } }
        private double _valor; public double valor { get => _valor; set { _valor = value; NotifyPropertyChanged(() => valor); } }
        private double _progreso; public double progreso { get => _progreso; set { _progreso = value; NotifyPropertyChanged(() => progreso); } }
        private string _unidad; public string unidad { get => _unidad; set { _unidad = value; NotifyPropertyChanged(() => unidad); } }
        private string _momento; public string momento { get => _momento; set { _momento = value; NotifyPropertyChanged(() => momento); } }

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
                    meta = meta,
                    valor = valor, 
                    progreso = progreso,
                    unidad = unidad, 
                    momento = momento

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
                    meta = meta,
                    valor = valor,
                    progreso = progreso,
                    unidad = unidad,
                    momento = momento
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
            var meta = ToString(row, "meta");
            var valor = ToDouble(row, "valor");
            var progreso = ToDouble(row, "progreso");
            var unidad = ToString(row, "unidad");
            var momento = ToString(row, "momento");

            var _attributes = new { ID_proyecto, meta, valor, progreso, unidad, momento };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearMeta { Owner = Application.Current.MainWindow };
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
                meta = "";
                valor = 0;
                progreso = 0;
                unidad = "";
                momento = "";
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
                editarElemento = new EditarMeta { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                    meta = Convert.ToString(row["meta"]);
                    valor = Convert.ToDouble(row["valor"]);
                    progreso = Convert.ToDouble(row["progreso"]);
                    unidad = Convert.ToString(row["unidad"]);
                    momento = Convert.ToString(row["momento"]);
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