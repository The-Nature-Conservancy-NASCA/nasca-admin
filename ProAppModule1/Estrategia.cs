using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Estrategia:Element
    {
        //fields
        public ProWindow1 crearElemento = null;  // create new element window
        private Update editarElemento = null; // update element window

        // constructor
        public Estrategia():base()
        {
            Index=3;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Estrategia";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
            Service = $"{serviceURL}/{Index}";

            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);
            AddNewRowCommand = new RelayCommand(() => { AddNewRow(AddRow); crearElemento?.Close(); }, () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => { UpdateSelectedRow(Objectid, UpdateRow); editarElemento?.Close(); }, () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);

            // Commands with effect on child
            SelectionCommand = new RelayCommand(() => OnElementoSelecionado(), () => true);

            Columns = new List<string> { "OBJECTID", "nombre", "descripcion", "ID_estrategia", "color", "fondo", "icono" };

            // Parent relationships
            // custom parent relationships here (view Proyecto)

        }

        // Child relationships
        private Proyecto _proyecto; public Proyecto Proyecto { get => _proyecto; set { _proyecto = value; NotifyPropertyChanged(() => Proyecto); } }

        /// Properties
        private string name; public string Name { get => name; set { name = value; NotifyPropertyChanged(() => Name); } }
        private string description; public string Description { get => description; set { description = value; NotifyPropertyChanged(() => Description); } }
        private string id; public string Id { get => id; set { id = value; NotifyPropertyChanged(() => Id); } }
        private string color; public string Color { get => color; set { color = value; NotifyPropertyChanged(() => Color); } }
        private string image; public string Image { get => image; set { image = value; NotifyPropertyChanged(() => Image); } }
        private string icon; public string Icon { get => icon; set { icon = value; NotifyPropertyChanged(() => Icon); } }

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
                    ID_estrategia = Id,
                    nombre = Name,
                    descripcion = Description,
                    color = Color,
                    fondo = Image,
                    icono = Icon
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
                    ID_estrategia = Id,
                    nombre = Name,
                    descripcion = Description,
                    color = Color,
                    fondo = Image,
                    icono = Icon
                };
                // -----------------

                return _updateRow;
            }
        }

        // Methods
        public override object FormatAttributes(Row row)
        {

            // Custom fields here ----------------
            var ID_estrategia = ToString(row, "ID_estrategia");
            var nombre = ToString(row, "nombre");
            var descripcion = ToString(row, "descripcion");
            var color = ToString(row, "color");
            var fondo = ToString(row, "fondo");
            var icono = ToString(row, "icono");

            var _attributes = new { ID_estrategia, nombre, color, fondo, icono, descripcion };
            // ----------------------

            return _attributes;

        }

        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new ProWindow1 {Owner = Application.Current.MainWindow};
                crearElemento.Closed += (o, e) => { crearElemento = null; };

                // Custom default values here
                Id = "";
                Name = "";
                Description = "";
                Color = "";
                Image = "";
                Icon = "";
                //---------------------------

                crearElemento.DataContext = this;
                crearElemento.Show();

            }
        }

        // Method to show the Pro Window
        public void ShowProWindowUpdate()
        {
            {
                if (editarElemento != null)
                    return;
                editarElemento = new Update {Owner = Application.Current.MainWindow};
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];

                    // Custom default values here
                    Name = Convert.ToString(row["Nombre"]);
                    Description = Convert.ToString(row["Descripcion"]);
                    Id = Convert.ToString(row["ID_estrategia"]);
                    Color = Convert.ToString(row["Color"]);
                    Image = Convert.ToString(row["Fondo"]);
                    Icon = Convert.ToString(row["Icono"]);
                    //---------------------------

                }
                else
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de la tabla de {ElementName}", "Actualiazar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    editarElemento.Close();
                    return;
                }

                editarElemento.DataContext = this;
                editarElemento.Show();
            }
        }


        public void OnElementoSelecionado()
        {
            if (SelectedIndex < 0) return;
            var row = data.Rows[SelectedIndex];

            // Custom child dependency
            var id_estrategia = Convert.ToString(row["ID_estrategia"]);
            _proyecto.data.DefaultView.RowFilter = $"ID_estrategia = '{id_estrategia}'";
            //-----------------------

        }

        public override async void UnselectRow()
        {
            SelectedIndex = -1;

            // Custom child dependency
            _proyecto.data.DefaultView.RowFilter = null;
            await FillDataTable();
            NotifyPropertyChanged(() => _proyecto.data);
            //---------------------
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

            var where = $"ID_estrategia = '{Id}'";
            WebInteraction.DeleteFeatures(_proyecto.Service, where);
            _proyecto.LoadData();


        }

    }
}