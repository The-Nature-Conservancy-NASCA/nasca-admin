using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Desktop.Framework;
using System.Windows.Input;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Framework.Dialogs;

namespace ProAppModule1
{
    public class Proyecto : Element
    {

        // fields
        private Estrategia _estrategia;

        // constructor
        public Proyecto(Estrategia estrategia):base()
        {
            Index = 4;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Proyecto";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;
            Service = $"{serviceURL}/{Index}";

            SelectionCommand = new RelayCommand(() => _estrategia.OnEstrategiaSelecionada(), () => true);
            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            AddNewRowCommand = new RelayCommand(() => AddNewRow(), () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => UpdateSelectedRow(), () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);

            Columns = new List<string> { "OBJECTID", "ID_estrategia", "ID_proyecto", "nombre", "descripcion", "color", "fondo", "icono",
                "fecha_linea_base", "fecha_seguimiento1", "fecha_seguimiento2", "fecha_cierre",  "cierre"};

            // Relationships
            _estrategia = estrategia;

        }

        /// properties
        private string name; public string Name { get => name; set { name = value; NotifyPropertyChanged(() => Name); } }
        private string description; public string Description { get => description; set { description = value; NotifyPropertyChanged(() => Description); } }
        private string id; public string Id { get => id; set { id = value; NotifyPropertyChanged(() => Id); } }
        private string id_estrategia; public string Id_estrategia { get => id_estrategia; set { id_estrategia = value; NotifyPropertyChanged(() => Id_estrategia); } }
        private string color; public string Color { get => color; set { color = value; NotifyPropertyChanged(() => Color); } }
        private string image; public string Image { get => image; set { image = value; NotifyPropertyChanged(() => Image); } }
        private string icon; public string Icon { get => icon; set { icon = value; NotifyPropertyChanged(() => Icon); } }
        private string fechalineabase; public string FechaLineaBase { get => fechalineabase; set { fechalineabase = value; NotifyPropertyChanged(() => FechaLineaBase); } }
        private string fechaseguimiento1; public string FechaSeguimiento1 { get => fechaseguimiento1; set { fechaseguimiento1 = value; NotifyPropertyChanged(() => FechaSeguimiento1); } }
        private string fechaseguimiento2; public string FechaSeguimiento2 { get => fechaseguimiento2; set { fechaseguimiento2 = value; NotifyPropertyChanged(() => FechaSeguimiento2); } }
        private string fechacierre; public string FechaCierre { get => fechacierre; set { fechacierre = value; NotifyPropertyChanged(() => FechaCierre); } }
        private int cierre; public int Cierre { get => cierre; set { cierre = value; NotifyPropertyChanged(() => Cierre); } }

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
            var ID_proyecto = ToString(row, "ID_proyecto");
            var nombre = ToString(row, "nombre");
            var descripcion = ToString(row, "descripcion");
            var descripcion_carbono = ToString(row, "descripcion_carbono");
            var descripcion_coberturas = ToString(row, "descripcion_coberturas");
            var descripcion_biodiversidad = ToString(row, "descripcion_biodiversidad");
            var descripcion_implementaciones = ToString(row, "descripcion_implementaciones");
            var descripcion_aliados = ToString(row, "descripcion_aliados");
            var descripcion_contribuciones = ToString(row, "descripcion_contribuciones");
            var descripcion_metas = ToString(row, "descripcion_metas");
            var descripcion_participantes = ToString(row, "descripcion_participantes");
            var color = ToString(row, "color");
            var fondo = ToString(row, "fondo");
            var icono = ToString(row, "icono");
            var fecha_linea_base = ToDate(row, "fecha_linea_base");
            var fecha_seguimiento1 = ToDate(row, "fecha_seguimiento1");
            var fecha_seguimiento2 = ToDate(row, "fecha_seguimiento2");
            var fecha_cierre = ToDate(row, "fecha_cierre");
            var cierre = ToInt(row, "cierre");

            var _attributes = new
            {
                ID_estrategia, ID_proyecto, nombre, descripcion, descripcion_carbono, descripcion_coberturas, descripcion_biodiversidad, descripcion_implementaciones,
                descripcion_aliados, descripcion_contribuciones, descripcion_metas, descripcion_participantes, color = color, fondo, icono, fecha_linea_base, fecha_seguimiento1,
                fecha_seguimiento2, fecha_cierre, cierre

            };
            return _attributes;
        }


        // Method to show the Pro Window
        private CrearProyecto crearProyecto = null;
        public void ShowWindow()
        {
            {
                //already open?
                if (crearProyecto != null)
                    return;
                crearProyecto = new CrearProyecto {Owner = Application.Current.MainWindow};
                crearProyecto.Closed += (o, e) => { crearProyecto = null; };

                if (_estrategia.SelectedIndex >= 0)
                {
                    var row = _estrategia.data.Rows[_estrategia.SelectedIndex];
                    Id_estrategia = Convert.ToString(row["ID_estrategia"]);
                }

                // Default values
                Name = "";
                Description = "";
                Color = "";
                Image = "";
                Icon = "";
                Id_estrategia = Id_estrategia;
                Id = "";
                FechaLineaBase = "";
                FechaSeguimiento1 = "";
                FechaSeguimiento2 = "";
                FechaCierre = "";
                Cierre = 2020;

                crearProyecto.DataContext = this;
                crearProyecto.Show();

            }

        }

        // Method to show the Pro Window
        private EditarProyecto editarProyecto = null;
        public void ShowProWindowUpdate()

        {
            {
                //already open?
                if (editarProyecto != null)
                    return;
                editarProyecto = new EditarProyecto {Owner = Application.Current.MainWindow};
                editarProyecto.Closed += (o, e) => { editarProyecto = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    Name = Convert.ToString(row["Nombre"]);
                    Description = Convert.ToString(row["Descripcion"]);
                    Color = Convert.ToString(row["Color"]);
                    Image = Convert.ToString(row["Fondo"]);
                    Icon = Convert.ToString(row["Icono"]);
                    Id_estrategia = Convert.ToString(row["ID_estrategia"]);
                    Id = Convert.ToString(row["ID_proyecto"]);
                    FechaLineaBase = (FechaLineaBase == null) ? null : UnixTimeStampToDateString(Convert.ToString(row["fecha_linea_base"]));
                    FechaSeguimiento1 = (FechaSeguimiento1 == null) ? null : UnixTimeStampToDateString(Convert.ToString(row["fecha_seguimiento1"]));
                    FechaSeguimiento2 = (FechaSeguimiento2 == null) ? null : UnixTimeStampToDateString(Convert.ToString(row["fecha_seguimiento2"]));
                    FechaCierre = (FechaCierre == null) ? null : UnixTimeStampToDateString(Convert.ToString(row["fecha_cierre"]));
                    Cierre = StringToInt(Convert.ToString(row["cierre"]));
                }
                else
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de {ElementName}", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    editarProyecto.Close();
                    return;
                }

                editarProyecto.DataContext = this;
                editarProyecto.Show();

            }
        }


        public async void UpdateSelectedRow()
        {
            if (SelectedIndex >= 0)
            {

                var row = data.Rows[SelectedIndex];

                if (_estrategia.SelectedIndex >= 0)
                {
                    var rowEstrategia = _estrategia.data.Rows[SelectedIndex];
                    Id_estrategia = Convert.ToString(rowEstrategia["ID_estrategia"]);

                }

                var _objectid = row[OidField];
                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    var _attributes = new
                    {
                        OBJECTID = objectid,
                        ID_estrategia = Id_estrategia,
                        ID_Proyecto = Id,
                        nombre = Name,
                        Descripcion = Description,
                        color = Color,
                        fondo = Image,
                        icono = Icon,
                        fecha_linea_base = (FechaLineaBase == null) ? null : (double?)DateStringToUnixTimeStamp(FechaLineaBase),
                        fecha_seguimiento1 = (FechaSeguimiento1 == null) ? null : (double?)DateStringToUnixTimeStamp(FechaSeguimiento1),
                        fecha_seguimiento2 = (FechaSeguimiento2 == null) ? null : (double?)DateStringToUnixTimeStamp(FechaSeguimiento2),
                        fecha_cierre = (FechaCierre == null) ? null : (double?)DateStringToUnixTimeStamp(FechaCierre),
                        cierre = Cierre
                    };
                    WebInteraction.UpdateFeatures(Service, objectid, _attributes);
                }

                await FillDataTable();
                NotifyPropertyChanged(() => data);
                editarProyecto?.Close();
            }
            else
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Seleccione el elemento que desea actualizar de {ElementName}", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                editarProyecto.Close();
            }
        }

        public async void AddNewRow()
        {
            var _attributes = new
            {
                ID_estrategia = Id_estrategia,
                ID_Proyecto = Id,
                nombre = Name,
                Descripcion = Description,
                color = Color,
                fondo = Image,
                icono = Icon,
                fecha_linea_base = (FechaLineaBase == null) ? null : (double?)DateStringToUnixTimeStamp(FechaLineaBase),
                fecha_seguimiento1 = (FechaSeguimiento1 == null) ? null : (double?)DateStringToUnixTimeStamp(FechaSeguimiento1),
                fecha_seguimiento2 = (FechaSeguimiento2 == null) ? null : (double?)DateStringToUnixTimeStamp(FechaSeguimiento2),
                fecha_cierre = (FechaCierre == null) ? null : (double?)DateStringToUnixTimeStamp(FechaCierre),
                cierre = Cierre
            };

            var objectid = WebInteraction.AddFeatures(Service, _attributes);
            await FillDataTable();
            NotifyPropertyChanged(() => data);
            crearProyecto?.Close();
        }

    }
}