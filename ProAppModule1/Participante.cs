using ArcGIS.Core.Data;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Catalog;

namespace ProAppModule1
{
    public class Participante : Element
    {
        // fields
        private Proyecto _proyecto;
        private CrearParticipante crearElemento = null; // create new element window
        private EditarParticipante editarElemento = null; // update element window

        // constructor
        public Participante(Proyecto proyecto) : base()
        {
            Index = 5;
            SelectedIndex = -1;
            OidField = "OBJECTID";
            ElementName = "Participante";
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

            Columns = new List<string> { "OBJECTID", "ID_proyecto", "numero_hombres", "numero_mujeres", "numero_indigenas", "numero_campesinos", "numero_sin_informacion", "momento" };

            // Parent relationships
            _proyecto = proyecto;

        }

        // Child relationships
        // custom child relationships here (view Estrategia)

        /// properties
        private string _ID_proyecto; public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; NotifyPropertyChanged(() => ID_proyecto); } }
        private int _numero_hombres; public int numero_hombres { get => _numero_hombres; set { _numero_hombres = value; NotifyPropertyChanged(() => numero_hombres); } }
        private int _numero_mujeres; public int numero_mujeres { get => _numero_mujeres; set { _numero_mujeres = value; NotifyPropertyChanged(() => numero_mujeres); } }
        private int _numero_indigenas; public int numero_indigenas { get => _numero_indigenas; set { _numero_indigenas = value; NotifyPropertyChanged(() => numero_indigenas); } }
        private int _numero_campesinos; public int numero_campesinos { get => _numero_campesinos; set { _numero_campesinos = value; NotifyPropertyChanged(() => numero_campesinos); } }
        private int _numero_sin_informacion; public int numero_sin_informacion { get => _numero_sin_informacion; set { _numero_sin_informacion = value; NotifyPropertyChanged(() => numero_sin_informacion); } }
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
                    numero_hombres = numero_hombres,
                    numero_mujeres = numero_mujeres,
                    numero_indigenas = numero_indigenas,
                    numero_campesinos = numero_campesinos,
                    numero_sin_informacion = numero_sin_informacion,
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
                    numero_hombres = numero_hombres,
                    numero_mujeres = numero_mujeres,
                    numero_indigenas = numero_indigenas,
                    numero_campesinos = numero_campesinos,
                    numero_sin_informacion = numero_sin_informacion,
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
            var numero_hombres = ToInt(row, "numero_hombres");
            var numero_mujeres = ToInt(row, "numero_mujeres");
            var numero_indigenas = ToInt(row, "numero_indigenas");
            var numero_campesinos = ToInt(row, "numero_campesinos");
            var numero_sin_informacion = ToInt(row, "numero_sin_informacion");
            var momento = ToString(row, "momento");

            var _attributes = new { ID_proyecto, numero_hombres, numero_mujeres, numero_indigenas, numero_campesinos, numero_sin_informacion, momento };
            // -------------------------

            return _attributes;
        }


        // Method to show the Pro Window
        public void ShowWindow()
        {
            {
                if (crearElemento != null)
                    return;
                crearElemento = new CrearParticipante { Owner = Application.Current.MainWindow };
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
                numero_hombres = 0;
                numero_mujeres = 0;
                numero_indigenas = 0;
                numero_campesinos = 0;
                numero_sin_informacion = 0;
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
                editarElemento = new EditarParticipante { Owner = Application.Current.MainWindow };
                editarElemento.Closed += (o, e) => { editarElemento = null; };

                if (SelectedIndex >= 0)
                {
                    var row = data.Rows[SelectedIndex];
                    ID_proyecto = Convert.ToString(row["ID_proyecto"]);
                    numero_hombres = Convert.ToInt32(row["numero_hombres"]);
                    numero_mujeres = Convert.ToInt32(row["numero_mujeres"]);
                    numero_indigenas = Convert.ToInt32(row["numero_indigenas"]);
                    numero_campesinos = Convert.ToInt32(row["numero_campesinos"]);
                    numero_sin_informacion = Convert.ToInt32(row["numero_sin_informacion"]);
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