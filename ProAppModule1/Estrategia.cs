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

        public Estrategia():base()
        {
            Index=3;
            Service = $"{serviceURL}/{Index}";
            //var Service2 = $"{serviceURL}/4";
            ElementName = "Estrategia";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;

            SelectionCommand = new RelayCommand(() => OnEstrategiaSelecionada(), () => true);
            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            AddNewRowCommand = new RelayCommand(() => AddNewRow(), () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => UpdateSelectedRow(), () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);
            SelectedIndex = -1;


            OidField = "OBJECTID";
            Columns = new List<string> { "OBJECTID", "nombre", "descripcion", "ID_estrategia", "color", "fondo", "icono" };

        }

        // Commands

        public ICommand SelectionCommand { get; }
        public ICommand ShowProWindow { get; }
        public ICommand ShowProWindowUpdateCommand { get; }
        public ICommand AddNewRowCommand { get; }
        public ICommand UpdateSelectedRowCommand { get; }
        public ICommand EliminateSelectedRow { get; }
        public ICommand UnselectRowCommand { get; }

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

        public override async void LoadData()
        {
            await FillDataTable();
            NotifyPropertyChanged(() => data);
        }

        public override async void UnselectRow()
        {
            SelectedIndex = -1;
            data.DefaultView.RowFilter = null;

            await FillDataTable();
            NotifyPropertyChanged(() => data);
        }

        // Method to show the Pro Window
        public ProWindow1 _prowindow1 = null;
        public void ShowWindow()
        {
            {
                //already open?
                if (_prowindow1 != null)
                    return;
                _prowindow1 = new ProWindow1 {Owner = Application.Current.MainWindow};
                _prowindow1.Closed += (o, e) => { _prowindow1 = null; };

                Id = "";
                Name = "";
                Description = "";
                Color = "";
                Image = "";
                Icon = "";

                _prowindow1.DataContext = this;
                _prowindow1.Show();

            }
        }

        // Method to show the Pro Window
        private Update _prowindow2 = null;

        public void ShowProWindowUpdate()

        {
            {
                //already open?
                if (_prowindow2 != null)
                    return;
                _prowindow2 = new Update {Owner = Application.Current.MainWindow};
                _prowindow2.Closed += (o, e) => { _prowindow2 = null; };

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
                    _prowindow2.Close();
                    return;
                }

                _prowindow2.DataContext = this;
                _prowindow2.Show();

            }
        }


        public async void EliminateRow()
        {
            if (SelectedIndex >= 0)
            {
                var answer = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("¿Desea eliminar el elemento seleccionado y sus registros relacionados?",
                    "Borrar registro", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                if (answer == MessageBoxResult.Yes)
                {
                    var row = data.Rows[SelectedIndex];
                    var _objectid = row[OidField];
                    //var estrategia = Convert.ToString(row["ID_estrategia"]);

                    if (_objectid != null)
                    {
                        var objectid = Convert.ToInt32(_objectid);
                        WebInteraction.DeleteFeatures(Service, objectid);
                    }

                    await FillDataTable();
                    NotifyPropertyChanged(() => data);

                }
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

                if (_prowindow2 != null)
                    _prowindow2.Close();

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

            if (_prowindow1 != null)
                _prowindow1.Close();
        }

        public void OnEstrategiaSelecionada()
        {

            if (SelectedIndex >= 0)
            {
                var row = data.Rows[SelectedIndex];
                var estrategia = Convert.ToString(row["ID_estrategia"]);
                //ProyectosDataTable.DefaultView.RowFilter = String.Format("ID_estrategia = '{0}'", estrategia);
            }
        }

        /// fields
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged(() => Name);
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                NotifyPropertyChanged(() => Description);
            }
        }

        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged(() => Id);
            }
        }

        private string color;
        public string Color
        {
            get => color;
            set
            {
                color = value;
                NotifyPropertyChanged(() => Color);
            }
        }

        private string image;
        public string Image
        {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged(() => Image);
            }
        }

        private string icon;
        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                NotifyPropertyChanged(() => Icon);
            }
        }

    }
}