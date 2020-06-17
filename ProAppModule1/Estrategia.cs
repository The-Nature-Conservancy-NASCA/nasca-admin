using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows;
using System.Windows.Documents;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Framework.Dialogs;

namespace ProAppModule1
{
    public class Estrategia:Element
    {

        // constructor
        public Estrategia():base()
        {
            Index=3;
            OidField = "OBJECTID";
            ElementName = "Estrategia";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
            Service = $"{serviceURL}/{Index}";

            SelectionCommand = new RelayCommand(() => OnEstrategiaSelecionada(), () => true);
            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            AddNewRowCommand = new RelayCommand(() => AddNewRow(), () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => UpdateSelectedRow(), () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);
            SelectedIndex = -1;

            Columns = new List<string> { "OBJECTID", "nombre", "descripcion", "ID_estrategia", "color", "fondo", "icono" };

        }

        /// Properties
        private string name; public string Name { get => name; set { name = value; NotifyPropertyChanged(() => Name); } }
        private string description; public string Description { get => description; set { description = value; NotifyPropertyChanged(() => Description); } }
        private string id; public string Id { get => id; set { id = value; NotifyPropertyChanged(() => Id); } }
        private string color; public string Color { get => color; set { color = value; NotifyPropertyChanged(() => Color); } }
        private string image; public string Image { get => image; set { image = value; NotifyPropertyChanged(() => Image); } }
        private string icon; public string Icon { get => icon; set { icon = value; NotifyPropertyChanged(() => Icon); } }
        private Proyecto _proyecto; public Proyecto Proyecto { get => _proyecto; set { _proyecto = value; NotifyPropertyChanged(()=>Proyecto); } }


        // Commands
        public ICommand SelectionCommand { get; }
        public ICommand ShowProWindow { get; }
        public ICommand ShowProWindowUpdateCommand { get; }
        public ICommand AddNewRowCommand { get; }
        public ICommand UpdateSelectedRowCommand { get; }
        public ICommand EliminateSelectedRow { get; }
        public ICommand UnselectRowCommand { get; }


        // Methods
        public override object FormatAttributes(Row row)
        {

            var ID_estrategia = ToString(row, "ID_estrategia");
            var nombre = ToString(row, "nombre");
            var descripcion = ToString(row, "descripcion");
            var color = ToString(row, "color");
            var fondo = ToString(row, "fondo");
            var icono = ToString(row, "icono");

            var _attributes = new { ID_estrategia, nombre, color, fondo, icono, descripcion };
            return _attributes;

        }

        // Method to show the Pro Window
        public ProWindow1 crearEstrategia = null;
        public void ShowWindow()
        {
            {
                //already open?
                if (crearEstrategia != null)
                    return;
                crearEstrategia = new ProWindow1 {Owner = Application.Current.MainWindow};
                crearEstrategia.Closed += (o, e) => { crearEstrategia = null; };

                // Default values
                Id = "";
                Name = "";
                Description = "";
                Color = "";
                Image = "";
                Icon = "";

                crearEstrategia.DataContext = this;
                crearEstrategia.Show();

            }
        }

        // Method to show the Pro Window
        private Update editarEstrategia = null;
        public void ShowProWindowUpdate()
        {
            {
                //already open?
                if (editarEstrategia != null)
                    return;
                editarEstrategia = new Update {Owner = Application.Current.MainWindow};
                editarEstrategia.Closed += (o, e) => { editarEstrategia = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    Name = Convert.ToString(row["Nombre"]);
                    Description = Convert.ToString(row["Descripcion"]);
                    Id = Convert.ToString(row["ID_estrategia"]);
                    Color = Convert.ToString(row["Color"]);
                    Image = Convert.ToString(row["Fondo"]);
                    Icon = Convert.ToString(row["Icono"]);
                }
                else
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Seleccione el elemento que desea actualizar de la tabla de Estrategias", "Actualiazar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    editarEstrategia.Close();
                    return;
                }

                editarEstrategia.DataContext = this;
                editarEstrategia.Show();

            }
        }


        public async void UpdateSelectedRow()
        {
            if (SelectedIndex >= 0)
            {
                var row = data.Rows[SelectedIndex];
                var _objectid = row[OidField];
                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    var _attributes = new
                    {
                        OBJECTID = objectid,
                        ID_estrategia = Id,
                        nombre = Name,
                        descripcion = Description,
                        color = Color,
                        fondo = Image,
                        icono = Icon
                    };
                    WebInteraction.UpdateFeatures(Service, objectid, _attributes);
                }

                await FillDataTable();
                NotifyPropertyChanged(() => data);

                editarEstrategia?.Close();

            }
            else
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de {ElementName}", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                editarEstrategia.Close();
            }
        }

        public async void AddNewRow()
        {
            var _attributes = new
            {
                ID_estrategia = Id,
                nombre = Name,
                descripcion = Description,
                color = Color,
                fondo = Image,
                icono = Icon
            };
            var objectid = WebInteraction.AddFeatures(Service, _attributes);

            await FillDataTable();
            NotifyPropertyChanged(() => data);

            crearEstrategia?.Close();
        }

        public void OnEstrategiaSelecionada()
        {
            if (SelectedIndex < 0) return;
            var row = data.Rows[SelectedIndex];
            var estrategia = Convert.ToString(row["ID_estrategia"]);
            _proyecto.data.DefaultView.RowFilter = $"ID_estrategia = '{estrategia}'";
        }

        public override async void UnselectRow()
        {
            SelectedIndex = -1;
            _proyecto.data.DefaultView.RowFilter = null;

            await FillDataTable();
            NotifyPropertyChanged(() => _proyecto.data);
        }

    }
}