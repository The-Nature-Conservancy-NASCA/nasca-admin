using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using System.Data;
using System.Windows.Input;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Windows.Media;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Data;
using ArcGIS.Desktop.Core.Geoprocessing;
using System.Configuration;
using System.Collections.Specialized;

namespace ProAppModule1
{

    /// View Model for the dockpane
    internal class Dockpane1ViewModel : DockPane

    {
        // Dockpane id
        private const string _dockPaneID = "ProAppModule1_Dockpane1";

        // Constants
        //private const string _serviceURL = "https://services9.arcgis.com/LQG65AprqDvQfUnp/ArcGIS/rest/services/TNCServices4/FeatureServer"; // Production
        private const string _serviceURL = "https://services9.arcgis.com/LQG65AprqDvQfUnp/arcgis/rest/services/TNC_Pruebas/FeatureServer"; // Testing

        // Commands for estrategias

        private readonly ICommand _testCommand;
        public ICommand TestCommand => _testCommand;

        private readonly ICommand _estrategiasSelectionCommand;
        public ICommand EstrategiasSelectionCommand => _estrategiasSelectionCommand;

        private readonly ICommand _loadDataCommand;
        public ICommand LoadDataCommand => _loadDataCommand;

        private readonly ICommand _showProWindow;
        public ICommand ShowProWindow => _showProWindow;

        private readonly ICommand _showProWindowUpdateCommand;
        public ICommand ShowProWindowUpdateCommand => _showProWindowUpdateCommand;

        private readonly ICommand _addNewRowCommand;
        public ICommand AddNewRowCommand => _addNewRowCommand;

        private readonly ICommand _updateSelectedRowCommand;
        public ICommand UpdateSelectedRowCommand => _updateSelectedRowCommand;

        private readonly ICommand _eliminateSelectedRow;
        public ICommand EliminateSelectedRow => _eliminateSelectedRow;

        private readonly ICommand _unselectRowCommand;
        public ICommand UnselectRowCommand => _unselectRowCommand;

        private readonly ICommand _uploadEstrategiaCommand;
        public ICommand UploadEstrategiaCommand => _uploadEstrategiaCommand;

        private readonly ICommand _browseFileEstrategiaCommand;
        public ICommand BrowseFileEstrategiaCommand => _browseFileEstrategiaCommand;

        public readonly ICommand _attachmentEstrategiaCommand;
        public ICommand AttachmentEstrategiaCommand => _attachmentEstrategiaCommand;

        // Commands for proyectos

        private readonly ICommand _loadDataCommand_pry;
        public ICommand LoadDataCommand_pry => _loadDataCommand_pry;

        private readonly ICommand _showProWindow_pry;
        public ICommand ShowProWindow_pry => _showProWindow_pry;

        private readonly ICommand _showProWindowUpdateCommand_pry;
        public ICommand ShowProWindowUpdateCommand_pry => _showProWindowUpdateCommand_pry;

        private readonly ICommand _addNewRowCommand_pry;
        public ICommand AddNewRowCommand_pry => _addNewRowCommand_pry;

        private readonly ICommand _updateSelectedRowCommand_pry;
        public ICommand UpdateSelectedRowCommand_pry => _updateSelectedRowCommand_pry;

        private readonly ICommand _eliminateSelectedRow_pry;
        public ICommand EliminateSelectedRow_pry => _eliminateSelectedRow_pry;

        private readonly ICommand _uploadProyectoCommand;
        public ICommand UploadProyectoCommand => _uploadProyectoCommand;

        private readonly ICommand _browseFileProyectoCommand;
        public ICommand BrowseFileProyectoCommand => _browseFileProyectoCommand;

        //  Property for data storage
        private DataTable _estrategiasDataTable = new DataTable();
        public DataTable EstrategiasDataTable
        {
            get { return _estrategiasDataTable; }
            set { SetProperty(ref _estrategiasDataTable, value, () => EstrategiasDataTable); }
        }

        //  Property for data storage
        private DataTable _proyectosDataTable = new DataTable();
        public DataTable ProyectosDataTable
        {
            get { return _proyectosDataTable; }
            set { SetProperty(ref _proyectosDataTable, value, () => ProyectosDataTable); }
        }

        // Properties for estrategia

        private int _selectedIndex_pry;
        public int SelectedIndex_pry
        {
            get { return _selectedIndex_pry; }
            set
            {
                _selectedIndex_pry = value;
                NotifyPropertyChanged(() => SelectedIndex_pry);
            }
        }

        private string _name_pry;
        public string name_pry
        {
            get { return _name_pry; }
            set
            {
                _name_pry = value;
                NotifyPropertyChanged(() => name_pry);
            }
        }

        private string _desc_pry;
        public string desc_pry
        {
            get { return _desc_pry; }
            set
            {
                _desc_pry = value;
                NotifyPropertyChanged(() => desc_pry);
            }
        }

        private string _id_estra_pry;
        public string id_estra_pry
        {
            get { return _id_estra_pry; }
            set
            {
                _id_estra_pry = value;
                NotifyPropertyChanged(() => id_estra_pry);
            }
        }

        private string _id_proye_pry;
        public string id_proye_pry
        {
            get { return _id_proye_pry; }
            set
            {
                _id_proye_pry = value;
                NotifyPropertyChanged(() => id_proye_pry);
            }
        }

        private string _color_pry;
        public string color_pry
        {
            get { return _color_pry; }
            set
            {
                _color_pry = value;
                NotifyPropertyChanged(() => color_pry);
            }
        }

        private string _image_pry;
        public string image_pry
        {
            get { return _image_pry; }
            set
            {
                _image_pry = value;
                NotifyPropertyChanged(() => image_pry);
            }
        }

        private string _icon_pry;
        public string icon_pry
        {
            get { return _icon_pry; }
            set
            {
                _icon_pry = value;
                NotifyPropertyChanged(() => icon_pry);
            }
        }

        private string _estrategia_file;
        public string Estrategia_File
        {
            get { return _estrategia_file; }
            set
            {
                _estrategia_file = value;
                NotifyPropertyChanged(() => Estrategia_File);
            }
        }

        private Item _selectedItem_estrategia;
        public Item SelectedItem_Estrategia
        {
            get { return _selectedItem_estrategia; }
            set
            {
                _selectedItem_estrategia = value;
                NotifyPropertyChanged(() => SelectedItem_Estrategia);
            }
        }

        // properties for proyectos
        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(() => SelectedIndex);
            }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(() => name);
            }
        }

        private string _desc;
        public string desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                NotifyPropertyChanged(() => desc);
            }
        }

        private string _id_estra;
        public string id_estra
        {
            get { return _id_estra; }
            set
            {
                _id_estra = value;
                NotifyPropertyChanged(() => id_estra);
            }
        }

        private string _id_proye;
        public string id_proye
        {
            get { return _id_proye; }
            set
            {
                _id_proye = value;
                NotifyPropertyChanged(() => id_proye);
            }
        }

        private string _color;
        public string color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged(() => color);
            }
        }

        private string _image;
        public string image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged(() => image);
            }
        }

        private string _icon;
        public string icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                NotifyPropertyChanged(() => icon);
            }
        }

        private string _proyecto_file;
        public string Proyecto_File
        {
            get { return _proyecto_file; }
            set
            {
                _proyecto_file = value;
                NotifyPropertyChanged(() => Proyecto_File);
            }
        }

        private Item _selectedItem_proyecto;
        public Item SelectedItem_Proyecto
        {
            get { return _selectedItem_proyecto; }
            set
            {
                _selectedItem_proyecto = value;
                NotifyPropertyChanged(() => SelectedItem_Proyecto);
            }
        }

        // Properties and commands for regiones
        private string _region_file;
        public string Region_File
        {
            get { return _region_file; }
            set {
                _region_file = value;
                NotifyPropertyChanged(() => Region_File);
            }
        }

        private Item _region_browsedItem;
        public Item RegionBrowsed_Item
        {
            get { return _region_browsedItem; }
            set
            {
                _region_browsedItem = value;
                NotifyPropertyChanged(() => RegionBrowsed_Item);
            }
        }

        private readonly ICommand _region_browseCommand;
        public ICommand Region_BrowseCommand => _region_browseCommand;

        private readonly ICommand _region_uploadCommand;
        public ICommand Region_UploadCommand => _region_uploadCommand;

        // Properties and commands for aliados
        private string _aliados_file;
        public string Aliados_File
        {
            get { return _aliados_file; }
            set
            {
                _aliados_file = value;
                NotifyPropertyChanged(() => Aliados_File);
            }
        }

        private Item _aliados_browsedItem;
        public Item AliadosBrowsed_Item
        {
            get { return _aliados_browsedItem; }
            set
            {
                _aliados_browsedItem = value;
                NotifyPropertyChanged(() => AliadosBrowsed_Item);
            }
        }

        private readonly ICommand _aliados_browseCommand;
        public ICommand Aliados_BrowseCommand => _aliados_browseCommand;

        private readonly ICommand _aliados_uploadCommand;
        public ICommand Aliados_UploadCommand => _aliados_uploadCommand;

        // Properties and commands for metas
        private string _metas_file;
        public string Metas_File { get { return _metas_file;} set { _metas_file = value; NotifyPropertyChanged(() => Metas_File);}}

        private Item _metas_browsedItem;
        public Item MetasBrowsed_Item {get { return _metas_browsedItem; }set {_metas_browsedItem = value; NotifyPropertyChanged(() => MetasBrowsed_Item);}}

        private readonly ICommand _metas_browseCommand;
        public ICommand Metas_BrowseCommand => _metas_browseCommand;

        private readonly ICommand _metas_uploadCommand;
        public ICommand Metas_UploadCommand => _metas_uploadCommand;

        // Properties and commands for participantes
        private string _participantes_file;
        public string Participantes_File
        {
            get { return _participantes_file; }
            set
            {
                _participantes_file = value;
                NotifyPropertyChanged(() => Participantes_File);
            }
        }

        private Item _participantes_browsedItem;
        public Item ParticipantesBrowsed_Item
        {
            get { return _participantes_browsedItem; }
            set
            {
                _participantes_browsedItem = value;
                NotifyPropertyChanged(() => ParticipantesBrowsed_Item);
            }
        }

        private readonly ICommand _participantes_browseCommand;
        public ICommand Participantes_BrowseCommand => _participantes_browseCommand;

        private readonly ICommand _participantes_uploadCommand;
        public ICommand Participantes_UploadCommand => _participantes_uploadCommand;

        // Properties and commands for contribuciones
        private string _contribuciones_file;
        public string Contribuciones_File
        {
            get { return _contribuciones_file; }
            set
            {
                _contribuciones_file = value;
                NotifyPropertyChanged(() => Contribuciones_File);
            }
        }

        private Item _contribuciones_browsedItem;
        public Item ContribucionesBrowsed_Item
        {
            get { return _contribuciones_browsedItem; }
            set
            {
                _contribuciones_browsedItem = value;
                NotifyPropertyChanged(() => ContribucionesBrowsed_Item);
            }
        }

        private readonly ICommand _contribuciones_browseCommand;
        public ICommand Contribuciones_BrowseCommand => _contribuciones_browseCommand;

        private readonly ICommand _contribuciones_uploadCommand;
        public ICommand Contribuciones_UploadCommand => _contribuciones_uploadCommand;

        // Properties and commands for predios
        private string _predio_file;
        public string Predio_File
        {
            get { return _predio_file; }
            set
            {
                _predio_file = value;
                NotifyPropertyChanged(() => Predio_File);
            }
        }

        private Item _predio_browsedItem;
        public Item PredioBrowsed_Item
        {
            get { return _predio_browsedItem; }
            set
            {
                _predio_browsedItem = value;
                NotifyPropertyChanged(() => PredioBrowsed_Item);
            }
        }

        private readonly ICommand _predio_browseCommand;
        public ICommand Predio_BrowseCommand => _predio_browseCommand;

        private readonly ICommand _predio_uploadCommand;
        public ICommand Predio_UploadCommand => _predio_uploadCommand;

        // Properties and commands for Implementacion
        private string _implementacion_file;
        public string Implementacion_File
        {
            get { return _implementacion_file; }
            set
            {
                _implementacion_file = value;
                NotifyPropertyChanged(() => Implementacion_File);
            }
        }

        private Item _implementacion_browsedItem;
        public Item ImplementacionBrowsed_Item
        {
            get { return _implementacion_browsedItem; }
            set
            {
                _implementacion_browsedItem = value;
                NotifyPropertyChanged(() => ImplementacionBrowsed_Item);
            }
        }

        private readonly ICommand _implementacion_browseCommand;
        public ICommand Implementacion_BrowseCommand => _implementacion_browseCommand;

        private readonly ICommand _implementacion_uploadCommand;
        public ICommand Implementacion_UploadCommand => _implementacion_uploadCommand;

        // Properties and commands for Cobertura
        private string _cobertura_file;
        public string Cobertura_File
        {
            get { return _cobertura_file; }
            set
            {
                _cobertura_file = value;
                NotifyPropertyChanged(() => Cobertura_File);
            }
        }

        private Item _cobertura_browsedItem;
        public Item CoberturaBrowsed_Item
        {
            get { return _cobertura_browsedItem; }
            set
            {
                _cobertura_browsedItem = value;
                NotifyPropertyChanged(() => CoberturaBrowsed_Item);
            }
        }

        private readonly ICommand _cobertura_browseCommand;
        public ICommand Cobertura_BrowseCommand => _cobertura_browseCommand;

        private readonly ICommand _cobertura_uploadCommand;
        public ICommand Cobertura_UploadCommand => _cobertura_uploadCommand;

        // Properties and commands for Carbono
        private string _carbono_file;
        public string Carbono_File
        {
            get { return _carbono_file; }
            set
            {
                _carbono_file = value;
                NotifyPropertyChanged(() => Carbono_File);
            }
        }

        private Item _carbono_browsedItem;
        public Item CarbonoBrowsed_Item
        {
            get { return _carbono_browsedItem; }
            set
            {
                _carbono_browsedItem = value;
                NotifyPropertyChanged(() => CarbonoBrowsed_Item);
            }
        }

        private readonly ICommand _carbono_browseCommand;
        public ICommand Carbono_BrowseCommand => _carbono_browseCommand;

        private readonly ICommand _carbono_uploadCommand;
        public ICommand Carbono_UploadCommand => _carbono_uploadCommand;

        // Properties and commands for Colores
        private string _colores_file;
        public string Colores_File
        {
            get { return _colores_file; }
            set
            {
                _colores_file = value;
                NotifyPropertyChanged(() => Colores_File);
            }
        }

        private Item _colores_browsedItem;
        public Item ColoresBrowsed_Item
        {
            get { return _colores_browsedItem; }
            set
            {
                _colores_browsedItem = value;
                NotifyPropertyChanged(() => ColoresBrowsed_Item);
            }
        }

        private readonly ICommand _colores_browseCommand;
        public ICommand Colores_BrowseCommand => _colores_browseCommand;

        private readonly ICommand _colores_uploadCommand;
        public ICommand Colores_UploadCommand => _colores_uploadCommand;

        // Properties and commands for Biodiversidad
        private string _biodiversidad_file;
        public string Biodiversidad_File
        {
            get { return _biodiversidad_file; }
            set
            {
                _biodiversidad_file = value;
                NotifyPropertyChanged(() => Biodiversidad_File);
            }
        }

        private Item _biodiversidad_browsedItem;
        public Item BiodiversidadBrowsed_Item
        {
            get { return _biodiversidad_browsedItem; }
            set
            {
                _biodiversidad_browsedItem = value;
                NotifyPropertyChanged(() => BiodiversidadBrowsed_Item);
            }
        }

        private readonly ICommand _biodiversidad_browseCommand;
        public ICommand Biodiversidad_BrowseCommand => _biodiversidad_browseCommand;

        private readonly ICommand _biodiversidad_uploadCommand;
        public ICommand Biodiversidad_UploadCommand => _biodiversidad_uploadCommand;

        // Properties and commands for Carrusel
        private string _carrusel_file;
        public string Carrusel_File
        {
            get { return _carrusel_file; }
            set
            {
                _carrusel_file = value;
                NotifyPropertyChanged(() => Carrusel_File);
            }
        }

        private Item _carrusel_browsedItem;
        public Item CarruselBrowsed_Item
        {
            get { return _carrusel_browsedItem; }
            set
            {
                _carrusel_browsedItem = value;
                NotifyPropertyChanged(() => CarruselBrowsed_Item);
            }
        }

        private readonly ICommand _carrusel_browseCommand;
        public ICommand Carrusel_BrowseCommand => _carrusel_browseCommand;

        private readonly ICommand _carrusel_uploadCommand;
        public ICommand Carrusel_UploadCommand => _carrusel_uploadCommand;


        // Dock Panel Constructor 
        protected Dockpane1ViewModel()

        {
            // Set up commands region
            _region_browseCommand = new RelayCommand(() => BrowseRegion(), () => true);
            _region_uploadCommand = new RelayCommand(() => UploadRegion(), () => true);

            // Set up commands aliados
            _aliados_browseCommand = new RelayCommand(() => BrowseAliados(), () => true);
            _aliados_uploadCommand = new RelayCommand(() => UploadAliados(), () => true);

            // Set up commands metas
            _metas_browseCommand = new RelayCommand(() => BrowseMetas(), () => true);
            _metas_uploadCommand = new RelayCommand(() => UploadMetas(), () => true);

            // Set up commands participantes
            _participantes_browseCommand = new RelayCommand(() => BrowseParticipantes(), () => true);
            _participantes_uploadCommand = new RelayCommand(() => UploadParticipantes(), () => true);

            // Set up commands contribuciones
            _contribuciones_browseCommand = new RelayCommand(() => BrowseContribuciones(), () => true);
            _contribuciones_uploadCommand = new RelayCommand(() => UploadContribuciones(), () => true);

            // Set up commands predios
            _predio_browseCommand = new RelayCommand(() => BrowsePredio(), () => true);
            _predio_uploadCommand = new RelayCommand(() => UploadPredio(), () => true);

            // Set up commands implementaciones
            _implementacion_browseCommand = new RelayCommand(() => BrowseImplementacion(), () => true);
            _implementacion_uploadCommand = new RelayCommand(() => UploadImplementacion(), () => true);

            // Set up commands coberturas
            _cobertura_browseCommand = new RelayCommand(() => BrowseCobertura(), () => true);
            _cobertura_uploadCommand = new RelayCommand(() => UploadCobertura(), () => true);

            // Set up commands carbono
            _carbono_browseCommand = new RelayCommand(() => BrowseCarbono(), () => true);
            _carbono_uploadCommand = new RelayCommand(() => UploadCarbono(), () => true);

            // Set up commands colores
            _colores_browseCommand = new RelayCommand(() => BrowseColores(), () => true);
            _colores_uploadCommand = new RelayCommand(() => UploadColores(), () => true);

            // Set up commands biodiversidad
            _biodiversidad_browseCommand = new RelayCommand(() => BrowseBiodiversidad(), () => true);
            _biodiversidad_uploadCommand = new RelayCommand(() => UploadBiodiversidad(), () => true);

            // Set up commands carrusel
            _carrusel_browseCommand = new RelayCommand(() => BrowseCarrusel(), () => true);
            _carrusel_uploadCommand = new RelayCommand(() => UploadCarrusel(), () => true);


            // Set up commands estrategia
            _estrategiasSelectionCommand = new RelayCommand(() => OnEstrategiaSelecionada(), () => true);
            _loadDataCommand = new RelayCommand(() => LoadData(), () => true);
            _showProWindow = new RelayCommand(() => ShowWindow(), () => true);
            _showProWindowUpdateCommand = new RelayCommand(() => ShowProWindowUpdate(), () => true);
            _addNewRowCommand = new RelayCommand(() => AddNewRow(), () => true);
            _eliminateSelectedRow = new RelayCommand(() => EliminateRow(), () => true);
            _updateSelectedRowCommand = new RelayCommand(() => UpdateSelectedRow(), () => true);
            _unselectRowCommand = new RelayCommand(() => UnselectRow(), ()=> true);
            _uploadEstrategiaCommand = new RelayCommand(() => UploadEstrategia(), () => true);
            _browseFileEstrategiaCommand = new RelayCommand(() => BrowseFileEstrategia(), () => true);
            //_attachmentEstrategiaCommand = new RelayCommand(() => AttachmentEstrategia(), () => true);

            // Set up commands proyecto
            _loadDataCommand_pry = new RelayCommand(() => LoadData_pry(), () => true);
            _showProWindow_pry = new RelayCommand(() => ShowWindow_pry(), () => true);
            _showProWindowUpdateCommand_pry = new RelayCommand(() => ShowProWindowUpdate_pry(), () => true);
            _addNewRowCommand_pry = new RelayCommand(() => AddNewRow_pry(), () => true);
            _eliminateSelectedRow_pry = new RelayCommand(() => EliminateRow_pry(), () => true);
            _updateSelectedRowCommand_pry = new RelayCommand(() => UpdateSelectedRow_pry(), () => true);
            _uploadProyectoCommand = new RelayCommand(() => UploadProyecto(), () => true);
            _browseFileProyectoCommand = new RelayCommand(() => BrowseFileProyecto(), () => true);

            // Load data 
            LoadData();
            LoadData_pry();

            // Startup properties
            SelectedIndex = -1;
            SelectedIndex_pry = -1;
        }

        private void UploadCarrusel()
        {
            var service = String.Format("{0}/10", _serviceURL);

            try
            {
                if (CarruselBrowsed_Item != null)
                {
                    if (CarruselBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(CarruselBrowsed_Item, service, "ProAppModule1.Carrusel");
                    else
                        LocalInteraction.UploadTable(CarruselBrowsed_Item, service, "ProAppModule1.Carrusel");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseCarrusel()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Carrusel de imágenes",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                CarruselBrowsed_Item = openFeatureClass.Items.First();
                _carrusel_file = CarruselBrowsed_Item.Path;
                NotifyPropertyChanged(() => _carrusel_file);
            }
        }

        private void UploadBiodiversidad()
        {
            var service = String.Format("{0}/2", _serviceURL);
            try
            {
                if (BiodiversidadBrowsed_Item != null)
                {
                    if (BiodiversidadBrowsed_Item.Type == "Shapefile")
                        LocalInteraction.UploadShapefile(BiodiversidadBrowsed_Item, service, "ProAppModule1.Biodiversidad");
                    else
                        LocalInteraction.UploadFeatureClass(BiodiversidadBrowsed_Item, service, "ProAppModule1.Biodiversidad");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseBiodiversidad()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione el shape file o el feature class de puntos con los datos de Biodiversidad",
                Filter = ItemFilters.featureClasses_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                BiodiversidadBrowsed_Item = openFeatureClass.Items.First();
                _biodiversidad_file = BiodiversidadBrowsed_Item.Path;
                NotifyPropertyChanged(() => _biodiversidad_file);
            }
        }

        private void UploadColores()
        {
            var service = String.Format("{0}/12", _serviceURL);

            try
            {
                if (ColoresBrowsed_Item != null)
                {
                    if (ColoresBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(ColoresBrowsed_Item, service, "ProAppModule1.Color");
                    else
                        LocalInteraction.UploadTable(ColoresBrowsed_Item, service, "ProAppModule1.Color");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseColores()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Colores",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                ColoresBrowsed_Item = openFeatureClass.Items.First();
                _colores_file = ColoresBrowsed_Item.Path;
                NotifyPropertyChanged(() => Colores_File);
            }
        }

        private void UploadCarbono()
        {
            var service = String.Format("{0}/11", _serviceURL);

            try
            {
                if (CarbonoBrowsed_Item != null)
                {
                    if (CarbonoBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(CarbonoBrowsed_Item, service, "ProAppModule1.Carbono");
                    else
                        LocalInteraction.UploadTable(CarbonoBrowsed_Item, service, "ProAppModule1.Carbono");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseCarbono()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Carbono",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                CarbonoBrowsed_Item = openFeatureClass.Items.First();
                _carbono_file = CarbonoBrowsed_Item.Path;
                NotifyPropertyChanged(() => Carbono_File);
            }
        }

        private void UploadCobertura()
        {
            var service = String.Format("{0}/13", _serviceURL);

            try
            {
                if (CoberturaBrowsed_Item != null)
                {
                    if (CoberturaBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(CoberturaBrowsed_Item, service, "ProAppModule1.Cobertura");
                    else
                        LocalInteraction.UploadTable(CoberturaBrowsed_Item, service, "ProAppModule1.Cobertura");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseCobertura()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Cobertura",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                CoberturaBrowsed_Item = openFeatureClass.Items.First();
                _cobertura_file = CoberturaBrowsed_Item.Path;
                NotifyPropertyChanged(() => Cobertura_File);
            }
        }

        private void UploadImplementacion()
        {
            var service = String.Format("{0}/8", _serviceURL);

            try
            {
                if (ImplementacionBrowsed_Item != null)
                {
                    if (ImplementacionBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(ImplementacionBrowsed_Item, service, "ProAppModule1.Implementacion");
                    else
                        LocalInteraction.UploadTable(ImplementacionBrowsed_Item, service, "ProAppModule1.Implementacion");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseImplementacion()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Implementación",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                ImplementacionBrowsed_Item = openFeatureClass.Items.First();
                _implementacion_file = ImplementacionBrowsed_Item.Path;
                NotifyPropertyChanged(() => Implementacion_File);
            }
        }

        private void UploadPredio()
        {
            var service = String.Format("{0}/1", _serviceURL);
            try
            {
                if (Region_File != null)
                {
                    if (RegionBrowsed_Item.Type == "Shapefile")
                        LocalInteraction.UploadShapefile(RegionBrowsed_Item, service, "ProAppModule1.Predio");
                    else
                        LocalInteraction.UploadFeatureClass(RegionBrowsed_Item, service, "ProAppModule1.Predio");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowsePredio()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione el shape file o el feature class de polígonos con los datos de Predio",
                Filter = ItemFilters.featureClasses_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                PredioBrowsed_Item = openFeatureClass.Items.First();
                _predio_file = PredioBrowsed_Item.Path;
                NotifyPropertyChanged(() => Predio_File);
            }
        }

        private void UploadContribuciones()
        {
            var service = String.Format("{0}/9", _serviceURL);

            try
            {
                if (ContribucionesBrowsed_Item != null)
                {
                    if (ContribucionesBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(ContribucionesBrowsed_Item, service, "ProAppModule1.Contribucion");
                    else
                        LocalInteraction.UploadTable(ContribucionesBrowsed_Item, service, "ProAppModule1.Contribucion");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseContribuciones()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Contribuciones",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                ContribucionesBrowsed_Item = openFeatureClass.Items.First();
                _contribuciones_file = ContribucionesBrowsed_Item.Path;
                NotifyPropertyChanged(() => Contribuciones_File);
            }
        }

        private void UploadParticipantes()
        {
            var service = String.Format("{0}/5", _serviceURL);

            try
            {
                if (ParticipantesBrowsed_Item != null)
                {
                    if (ParticipantesBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(ParticipantesBrowsed_Item, service, "ProAppModule1.Participante");
                    else
                        LocalInteraction.UploadTable(ParticipantesBrowsed_Item, service, "ProAppModule1.Participante");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseParticipantes()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Participantes",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                ParticipantesBrowsed_Item = openFeatureClass.Items.First();
                _participantes_file = ParticipantesBrowsed_Item.Path;
                NotifyPropertyChanged(() => Participantes_File);
            }
        }

        private void UploadAliados()
        {
            var service = String.Format("{0}/7", _serviceURL);

            try
            {
                if (AliadosBrowsed_Item != null)
                {
                    if (AliadosBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(AliadosBrowsed_Item, service, "ProAppModule1.Aliado");
                    else
                    {
                        LocalInteraction.UploadExcel(AliadosBrowsed_Item, service, "ProAppModule1.Aliado");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseAliados()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Aliados",
                //Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                AliadosBrowsed_Item = openFeatureClass.Items.First();
                _aliados_file = AliadosBrowsed_Item.Path;
                NotifyPropertyChanged(() => Aliados_File);
            }
        }

        private void UploadMetas()
        {
            var service = String.Format("{0}/6", _serviceURL);

            try
            {
                if (MetasBrowsed_Item != null)
                {
                    if (MetasBrowsed_Item.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(MetasBrowsed_Item, service, "ProAppModule1.Meta");
                    else
                    {
                        LocalInteraction.UploadExcel(MetasBrowsed_Item, service, "ProAppModule1.Meta");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        private void BrowseMetas()
        {
            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase con los datos de Metas",
                //Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                MetasBrowsed_Item = openFeatureClass.Items.First();
                _metas_file = MetasBrowsed_Item.Path;
                NotifyPropertyChanged(() => Metas_File);
            }
        }

        public void UploadRegion()
        {
            var service = String.Format("{0}/0", _serviceURL);
            try
            {
                if (RegionBrowsed_Item != null)
                {
                    if (RegionBrowsed_Item.Type == "Shapefile")
                        LocalInteraction.UploadShapefile(RegionBrowsed_Item, service, "ProAppModule1.Region");
                    else
                        LocalInteraction.UploadFeatureClass(RegionBrowsed_Item, service, "ProAppModule1.Region");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        public void BrowseRegion()
        {

            var openFeatureClass = new OpenItemDialog()
            {
                Title = "Seleccione el shape file o el feature class de polígonos con los datos de la Región",
                Filter = ItemFilters.featureClasses_all
            };

            Nullable<bool> result = openFeatureClass.ShowDialog();
            if (result == true)
            {
                RegionBrowsed_Item = openFeatureClass.Items.First();
                _region_file = RegionBrowsed_Item.Path;
                NotifyPropertyChanged(() => Region_File);
            }
        }

        public void UploadEstrategia() {

            var service = String.Format("{0}/3", _serviceURL);

            try
            {
                if (SelectedItem_Estrategia != null)
                {
                    if (SelectedItem_Estrategia.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(SelectedItem_Estrategia, service, "ProAppModule1.Estrategia");
                    else
                        LocalInteraction.UploadExcel(SelectedItem_Estrategia, service, "ProAppModule1.Estrategia");

                    LoadData();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        public void BrowseFileEstrategia() {

            var openTable = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase que contiene los datos de estrategia",
                Filter = ItemFilters.tables_all
            };
            Nullable<bool> result = openTable.ShowDialog();
            if (result == true)
            {
                SelectedItem_Estrategia = openTable.Items.First();
                _estrategia_file = SelectedItem_Estrategia.Path;
                NotifyPropertyChanged(() => Estrategia_File);
            }
        }

        public void UploadProyecto()
        {
            var service = String.Format("{0}/4", _serviceURL);
            try
            {
                if (SelectedItem_Proyecto != null)
                {
                    if (SelectedItem_Proyecto.Type == "File Geodatabase Table")
                        LocalInteraction.UploadTable(SelectedItem_Proyecto, service, "ProAppModule1.Proyecto");
                    else
                        LocalInteraction.UploadExcel(SelectedItem_Proyecto, service, "ProAppModule1.Proyecto");

                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            return;
        }

        public void BrowseFileProyecto()
        {

            var openTable = new OpenItemDialog()
            {
                Title = "Seleccione la hoja de excel o la tabla de geodatabase que contiene los datos de Proyecto",
                Filter = ItemFilters.tables_all
            };

            Nullable<bool> result = openTable.ShowDialog();
            if (result == true)
            {
                SelectedItem_Proyecto = openTable.Items.First();
                _proyecto_file = SelectedItem_Proyecto.Path;
                NotifyPropertyChanged(() => Proyecto_File);
            }
        }


        public void UnselectRow() {

            SelectedIndex = -1;
            ProyectosDataTable.DefaultView.RowFilter = null;

        }


        public void OnEstrategiaSelecionada() {

            if (SelectedIndex >= 0) {
                var row = EstrategiasDataTable.Rows[SelectedIndex];
                var estrategia = Convert.ToString(row["ID_estrategia"]);
                //MessageBox.Show(String.Format("Estrategia seleccionada {0}", estrategia));
                ProyectosDataTable.DefaultView.RowFilter = String.Format("ID_estrategia = '{0}'", estrategia);
            }
        }

        public void EliminateRow() {

            var service = String.Format("{0}/3", _serviceURL);

            if (SelectedIndex >= 0)
            {
                var row = EstrategiasDataTable.Rows[SelectedIndex];
                var _objectid = row["OBJECTID"];
                var estrategia = Convert.ToString(row["ID_estrategia"]);

                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    WebInteraction.DeleteFeatures(service, objectid);
                }

                row.Delete();
                EstrategiasDataTable.AcceptChanges();
                NotifyPropertyChanged(() => EstrategiasDataTable);

                var service_pry = String.Format("{0}/4", _serviceURL);
                DataRow[] drr = ProyectosDataTable.Select("ID_estrategia='" + estrategia + "' ");
                for (int i = 0; i < drr.Length; i++)
                {
                    var _objectid_proyecto = Convert.ToInt32(drr[i]["OBJECTID"]);
                    WebInteraction.DeleteFeatures(service_pry,  _objectid_proyecto);
                    drr[i].Delete();
                }
                    
                ProyectosDataTable.AcceptChanges();
                NotifyPropertyChanged(() => ProyectosDataTable);
            }
        }

        public void UpdateSelectedRow() {

            var service = String.Format("{0}/3", _serviceURL);

            if (SelectedIndex >= 0)
            {
                var row = EstrategiasDataTable.Rows[SelectedIndex];
                var _objectid = row["OBJECTID"];

                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    var _attributes = new { ID_estrategia = id_estra, nombre = name, descripcion = desc, color = color, fondo = image, icono = icon };
                    WebInteraction.UpdateFeatures(service, objectid, _attributes);
                }

                row["Nombre"] = name;
                row["Descripcion"] = desc;
                row["ID_estrategia"] = id_estra;
                row["Color"] = color;
                row["Fondo"] = image;
                row["Icono"] = icon;

                EstrategiasDataTable.AcceptChanges();
                NotifyPropertyChanged(() => EstrategiasDataTable);

                if (_prowindow2 != null)
                    _prowindow2.Close();

            }
        }


        public void AddNewRow() {

            var service = String.Format("{0}/3", _serviceURL);
            var _attributes = new { ID_estrategia = id_estra, nombre = name, descripcion = desc, color = color, fondo = image, icono = icon };
            var objectid = WebInteraction.AddFeatures(service, _attributes);
            var _resultTable = EstrategiasDataTable;
            var addRow = _resultTable.NewRow();

            addRow["OBJECTID"] = objectid;
            addRow["ID_estrategia"] = id_estra;
            addRow["Nombre"] = name;
            addRow["Descripcion"] = desc;
            addRow["Color"] = color;
            addRow["Fondo"] = image;
            addRow["Icono"] = icon;

            _resultTable.Rows.Add(addRow);
            _estrategiasDataTable = _resultTable;
            NotifyPropertyChanged(() => EstrategiasDataTable);

            if (_prowindow1 != null)
                _prowindow1.Close();
        }

        public void EliminateRow_pry()
        {
            var service = String.Format("{0}/4", _serviceURL);
            if (SelectedIndex_pry >= 0)
            {
                var row = ProyectosDataTable.Rows[SelectedIndex_pry];

                var _objectid = row["OBJECTID"];
                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    WebInteraction.DeleteFeatures(service, objectid);
                }

                row.Delete();
                ProyectosDataTable.AcceptChanges();
                NotifyPropertyChanged(() => ProyectosDataTable);

            }
        }


        public void UpdateSelectedRow_pry()
        {
            var service = String.Format("{0}/4", _serviceURL);

            if (SelectedIndex_pry >= 0)
            {

                var row = ProyectosDataTable.Rows[SelectedIndex_pry];
                var rowEstrategia = EstrategiasDataTable.Rows[SelectedIndex];


                var _objectid = row["OBJECTID"];
                if (_objectid != null)
                {
                    var objectid = Convert.ToInt32(_objectid);
                    var _attributes = new { ID_estrategia = id_estra_pry, nombre = name_pry, descripcion = desc_pry, color = color_pry, fondo = image_pry, icono = icon_pry };
                    WebInteraction.UpdateFeatures(service, objectid, _attributes);
                }

                row["Nombre"] = name_pry;
                row["Descripcion"] = desc_pry;
                row["Color"] = color_pry;
                row["Fondo"] = image_pry;
                row["Icono"] = icon_pry;
                row["ID_estrategia"] = id_estra_pry;
                row["ID_proyecto"] = id_proye_pry;

                ProyectosDataTable.AcceptChanges();
                NotifyPropertyChanged(() => ProyectosDataTable);

                if (_prowindow2_pry != null)
                    _prowindow2_pry.Close();

            }

        }


        public void AddNewRow_pry()
        {
            var service = String.Format("{0}/4", _serviceURL);
            var _attributes = new { ID_estrategia = id_estra_pry, nombre = name_pry, descripcion = desc_pry, color = color_pry, fondo = image_pry, icono = icon_pry };
            var objectid = WebInteraction.AddFeatures(service, _attributes);

            var _resultTable = ProyectosDataTable;
            var addRow = _resultTable.NewRow();
            addRow["OBJECTID"] = objectid;
            addRow["ID_estrategia"] = id_estra_pry;
            addRow["ID_proyecto"] = id_proye_pry;
            addRow["Nombre"] = name_pry;
            addRow["Descripcion"] = desc_pry;
            addRow["Color"] = color_pry;
            addRow["Fondo"] = image_pry;
            addRow["Icono"] = icon_pry;
            _resultTable.Rows.Add(addRow);

            _proyectosDataTable = _resultTable;

            NotifyPropertyChanged(() => ProyectosDataTable);

            if (_prowindow1_pry != null)
                _prowindow1_pry.Close();

        }

        public void LoadData() {

            var _resultTable = new DataTable();
            _resultTable.Columns.Add(new DataColumn("OBJECTID"));
            _resultTable.Columns.Add(new DataColumn("Nombre"));
            _resultTable.Columns.Add(new DataColumn("Descripcion"));
            _resultTable.Columns.Add(new DataColumn("ID_estrategia"));
            _resultTable.Columns.Add(new DataColumn("Color"));
            _resultTable.Columns.Add(new DataColumn("Fondo"));
            _resultTable.Columns.Add(new DataColumn("Icono"));

            var service = String.Format("{0}/3", _serviceURL);

            var where = "1=1";
            var outFields = "*";
            var features = WebInteraction.Query(service, where, outFields);

            foreach (var feature in (System.Collections.ArrayList)features)
            {
                var feat = (Dictionary<string, object>)feature;
                var atts = (Dictionary<string, object>)feat["attributes"];

                var addRow = _resultTable.NewRow();
                addRow["OBJECTID"] = atts["OBJECTID"];
                addRow["ID_estrategia"] = atts["ID_estrategia"];
                addRow["Nombre"] = atts["nombre"];
                addRow["Descripcion"] = atts["descripcion"];
                addRow["Color"] = atts["color"];
                addRow["Fondo"] = atts["fondo"];
                addRow["Icono"] = atts["icono"];

                _resultTable.Rows.Add(addRow);
            }

            _estrategiasDataTable = _resultTable;
            NotifyPropertyChanged(() => EstrategiasDataTable);

        }

        // Show the DockPane.
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            pane.Activate();
        }

        // Method to show the Pro Window
        private ProWindow1 _prowindow1 = null;
        public void ShowWindow() {
            {
                //already open?
                if (_prowindow1 != null)
                    return;
                _prowindow1 = new ProWindow1();
                _prowindow1.Owner = FrameworkApplication.Current.MainWindow;
                _prowindow1.Closed += (o, e) => { _prowindow1 = null; };

                id_estra = "";
                name = "";
                desc = "";
                color = "";
                image = "";
                icon = "";

                _prowindow1.DataContext = this;

                _prowindow1.Show();
                //uncomment for modal
                //_prowindow1.ShowDialog();
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
                _prowindow2 = new Update();
                _prowindow2.Owner = FrameworkApplication.Current.MainWindow;
                _prowindow2.Closed += (o, e) => { _prowindow2 = null; };

                if (SelectedIndex >= 0)
                {

                    var row = EstrategiasDataTable.Rows[SelectedIndex];
                    name = Convert.ToString(row["Nombre"]);
                    desc = Convert.ToString(row["Descripcion"]);
                    id_estra = Convert.ToString(row["ID_estrategia"]);
                    color = Convert.ToString(row["Color"]);
                    image = Convert.ToString(row["Fondo"]);
                    icon = Convert.ToString(row["Icono"]);

                }
                else {

                    MessageBox.Show("Seleccione el elemento que desea actualizar de la tabla de Estrategias ");
                    _prowindow2.Close();
                }

                _prowindow2.DataContext = this;

                _prowindow2.Show();
                //uncomment for modal
                //_prowindow2.ShowDialog();
            }
        }



        // Method for Load Data to dockpane
        public void LoadData_pry()

        {
            var _resultTable = new DataTable();
            _resultTable.Columns.Add(new DataColumn("OBJECTID"));
            _resultTable.Columns.Add(new DataColumn("Nombre"));
            _resultTable.Columns.Add(new DataColumn("Descripcion"));
            _resultTable.Columns.Add(new DataColumn("ID_estrategia"));
            _resultTable.Columns.Add(new DataColumn("ID_proyecto"));
            _resultTable.Columns.Add(new DataColumn("Color"));
            _resultTable.Columns.Add(new DataColumn("Fondo"));
            _resultTable.Columns.Add(new DataColumn("Icono"));

            var service = String.Format("{0}/4", _serviceURL);
            var where = "1=1";
            var outFields = "*";
            var features = WebInteraction.Query(service, where, outFields);

            foreach (var feature in (System.Collections.ArrayList)features)
            {
                var feat = (Dictionary<string, object>)feature;
                var atts = (Dictionary<string, object>)feat["attributes"];

                var addRow = _resultTable.NewRow();
                addRow["OBJECTID"] = atts["OBJECTID"];
                addRow["ID_estrategia"] = atts["ID_estrategia"];
                addRow["ID_proyecto"] = atts["ID_proyecto"];
                addRow["Nombre"] = atts["nombre"];
                addRow["Descripcion"] = atts["descripcion"];
                addRow["Color"] = atts["color"];
                addRow["Fondo"] = atts["fondo"];
                addRow["Icono"] = atts["icono"];

                _resultTable.Rows.Add(addRow);
            }

            _proyectosDataTable = _resultTable;
            NotifyPropertyChanged(() => ProyectosDataTable);


        }


        // Method to show the Pro Window
        private CrearProyecto _prowindow1_pry = null;
        public void ShowWindow_pry()
        {
            {
                //already open?
                if (_prowindow1_pry != null)
                    return;
                _prowindow1_pry = new CrearProyecto();
                _prowindow1_pry.Owner = FrameworkApplication.Current.MainWindow;
                _prowindow1_pry.Closed += (o, e) => { _prowindow1_pry = null; };
                //_prowindow1.DataContext = EstrategiasDataTable; 

                var estrategia = "";
                if (SelectedIndex >= 0)
                {
                    var row = EstrategiasDataTable.Rows[SelectedIndex];
                    estrategia = Convert.ToString(row["ID_estrategia"]);

                }

                name_pry = "";
                desc_pry = "";
                color_pry = "";
                image_pry = "";
                icon_pry = "";
                id_estra_pry = estrategia;
                id_proye_pry = "";

                _prowindow1_pry.DataContext = this;
                _prowindow1_pry.Show();
                //uncomment for modal
                //_prowindow1.ShowDialog();
            }

        }

        // Method to show the Pro Window
        private EditarProyecto _prowindow2_pry = null;
        public void ShowProWindowUpdate_pry()

        {
            {
                //already open?
                if (_prowindow2_pry != null)
                    return;
                _prowindow2_pry = new EditarProyecto();
                _prowindow2_pry.Owner = FrameworkApplication.Current.MainWindow;
                _prowindow2_pry.Closed += (o, e) => { _prowindow2_pry = null; };
                //_prowindow1.DataContext = EstrategiasDataTable; 

                if (SelectedIndex_pry >= 0)
                {

                    var row = ProyectosDataTable.Rows[SelectedIndex_pry];
                    name_pry = Convert.ToString(row["Nombre"]);
                    desc_pry = Convert.ToString(row["Descripcion"]);
                    color_pry = Convert.ToString(row["Color"]);
                    image_pry = Convert.ToString(row["Fondo"]);
                    icon_pry = Convert.ToString(row["Icono"]);
                    id_estra_pry = Convert.ToString(row["ID_estrategia"]);
                    id_proye_pry = Convert.ToString(row["ID_proyecto"]);
                }
                else
                {
                    MessageBox.Show("Seleccione el elemento que desea actualizar de la tabla de Proyectos ");
                    _prowindow2_pry.Close();
                }

                _prowindow2_pry.DataContext = this;

                _prowindow2_pry.Show();
                //uncomment for modal
                //_prowindow2.ShowDialog();
            }
        }
    }

   
    /// Button implementation to show the DockPane.
    internal class Dockpane1_ShowButton : Button
    {
        protected override void OnClick()
        {
            Dockpane1ViewModel.Show();
        }
    }
}
