﻿using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
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

        //private Estrategia estrategia_pry;
        //public Estrategia Estrategia_pry { get; set; }

        public Proyecto():base()
        {
            Index = 4;
            Service = $"{serviceURL}/{Index}";
            ElementName = "Proyecto";
            ElementType = "la hoja de excel o la tabla de geodatabase";
            FilterType = ItemFilters.tables_all;

            //SelectionCommand = new RelayCommand(() => OnEstrategiaSelecionada(), () => true);
            ShowProWindow = new RelayCommand(() => ShowWindow(), () => true);
            ShowProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            AddNewRowCommand = new RelayCommand(() => AddNewRow(), () => true);
            UpdateSelectedRowCommand = new RelayCommand(() => UpdateSelectedRow(), () => true);
            EliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);
            UnselectRowCommand = new RelayCommand(() => UnselectRow(), () => true);
            SelectedIndex = -1;

            OidField = "OBJECTID";
            Columns = new List<string> { "OBJECTID", "ID_estrategia", "ID_proyecto", "nombre", "descripcion", "color", "fondo", "icono" };

            //Estrategia_pry = estrategia;

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
                descripcion_aliados, descripcion_contribuciones, descripcion_metas, descripcion_participantes, color, fondo, icono, fecha_linea_base, fecha_seguimiento1,
                fecha_seguimiento2, fecha_cierre, cierre

            };
            return _attributes;
        }

        public override async void LoadData()
        {
            await FillDataTable();
            NotifyPropertyChanged(() => data);
        }


        // Method to show the Pro Window
        private CrearProyecto _prowindow1_pry = null;

        public void ShowWindow()
        {
            {
                //already open?
                if (_prowindow1_pry != null)
                    return;
                _prowindow1_pry = new CrearProyecto {Owner = Application.Current.MainWindow};
                _prowindow1_pry.Closed += (o, e) => { _prowindow1_pry = null; };
                //_prowindow1.DataContext = EstrategiasDataTable; 

                var id_Estrategia = "";
                //if (estrategia_pry.SelectedIndex >= 0)
                //{
                //    var row = estrategia_pry.data.Rows[SelectedIndex];
                //    id_Estrategia = Convert.ToString(row["ID_estrategia"]);

                //}

                Name = "";
                Description = "";
                Color = "";
                Image = "";
                Icon = "";
                //Id_estrategia = id_Estrategia;
                Id = "";

                _prowindow1_pry.DataContext = this;
                _prowindow1_pry.Show();

            }

        }

        // Method to show the Pro Window
        private EditarProyecto _prowindow2_pry = null;

        public void ShowProWindowUpdate()

        {
            {
                //already open?
                if (_prowindow2_pry != null)
                    return;
                _prowindow2_pry = new EditarProyecto {Owner = Application.Current.MainWindow};
                _prowindow2_pry.Closed += (o, e) => { _prowindow2_pry = null; };

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
                }
                else
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Seleccione el elemento que desea actualizar de la tabla de Proyectos", "Actualizar registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    _prowindow2_pry.Close();
                    return;
                }

                _prowindow2_pry.DataContext = this;
                _prowindow2_pry.Show();

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

                //var id_Estrategia = "";
                //if (estrategia_pry.SelectedIndex >= 0)
                //{
                //    var rowEstrategia = estrategia_pry.data.Rows[SelectedIndex];
                //    id_Estrategia = Convert.ToString(rowEstrategia["ID_estrategia"]);

                //}

                var _objectid = row[OidField];
                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    var _attributes = new
                    {
                        OBJECTID = objectid,
                        //ID_estrategia = id_Estrategia,
                        ID_Proyecto = Id,
                        nombre = Name,
                        Descripcion = Description,
                        color = Color,
                        fondo = Image,
                        icono = Icon
                    };
                    WebInteraction.UpdateFeatures(Service, objectid, _attributes);
                }

                await FillDataTable();
                NotifyPropertyChanged(() => data);

                if (_prowindow2_pry != null)
                    _prowindow2_pry.Close();

            }
        }




        public async void AddNewRow()
        {
            var _attributes = new
            {
                //ID_estrategia = id_estra_pry,
                ID_Proyecto = Id,
                nombre = Name,
                descripcion = Description,
                color = Color,
                fondo = Image,
                icono = Icon
            };
            var objectid = WebInteraction.AddFeatures(Service, _attributes);

            await FillDataTable();
            NotifyPropertyChanged(() => data);

            if (_prowindow1_pry != null)
                _prowindow1_pry.Close();

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

        private string id_estrategia;
        public string Id_estrategia
        {
            get => id_estrategia;
            set
            {
                id_estrategia = value;
                NotifyPropertyChanged(() => Id_estrategia);
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