using ArcGIS.Core.Data;
using System;

namespace ProAppModule1
{
    public class ServicesModel
    {
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var unixTime = (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;

            return unixTime;
        }
    }

    public class Region
    {

        private string _ID_proyecto = "";
        private string _ID_region = "";
        private string _nombre = "";
        private double _fecha;
        //private double _area;

        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public string ID_region { get => _ID_region; set { _ID_region = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        public double fecha { get => _fecha; set { _fecha = value; } }
        //public double area { get => _area; set { _area = value; } }


        public Region(Row row)
        {

            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _ID_region = Convert.ToString(row["ID_region"]);
            _nombre = Convert.ToString(row["nombre"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds; 
            //_area = Convert.ToDouble(row["area"]);

        }

        public Region() { }
    }


    public class Estrategia
    {

        private string _ID_estrategia = "";
        private string _nombre = "";
        private string _color = "";
        private string _fondo = "";
        private string _icono = "";
        private string _descripcion = "";

        public string ID_estrategia { get => _ID_estrategia; set { _ID_estrategia = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        public string color { get => _color; set { _color = value; } }
        public string fondo { get => _fondo; set { _fondo = value; } }
        public string icono { get => _icono; set { _icono = value; } }
        public string descripcion { get => _descripcion; set { _descripcion = value; } }

        public Estrategia(Row row)
        {
            _ID_estrategia = Convert.ToString(row["ID_estrategia"]);
            _nombre = Convert.ToString(row["nombre"]);
            _color = Convert.ToString(row["color"]);
            _fondo = Convert.ToString(row["fondo"]);
            _icono = Convert.ToString(row["icono"]);
            _descripcion = Convert.ToString(row["descripcion"]);

        }

        public Estrategia() { }
    }

    public class Proyecto
    {

        private string _ID_proyecto = "";
        private string _ID_estrategia = "";
        private string _nombre = "";
        private string _descripcion = "";
        private string _descripcion_biodiversidad = "";
        private string _descripcion_carbono = "";
        private string _descripcion_coberturas = "";
        private string _descripcion_implementaciones = "";
        private string _descripcion_aliados = "";
        private string _descripcion_contribuciones = "";
        private string _descripcion_metas = "";
        private string _descripcion_participantes = "";
        private string _color = "";
        private string _fondo = "";
        private string _icono = "";
        private double _fecha_seguimiento1;
        private double _fecha_seguimiento2;
        private double _fecha_cierre;
        //private string _id_attachment = "";

        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public string ID_estrategia { get => _ID_estrategia; set { _ID_estrategia = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        public string descripcion { get => _descripcion; set { _descripcion = value; } }
        public string descripcion_carbono { get => _descripcion_carbono; set { _descripcion_carbono = value; } }
        public string descripcion_coberturas { get => _descripcion_coberturas; set { _descripcion_coberturas = value; } }
        public string descripcion_biodiversidad { get => _descripcion_biodiversidad; set { _descripcion_biodiversidad = value; } }
        public string descripcion_implementaciones { get => _descripcion_implementaciones; set { _descripcion_implementaciones = value; } }
        public string descripcion_aliados { get => _descripcion_aliados; set { _descripcion_aliados = value; } }
        public string descripcion_contribuciones { get => _descripcion_contribuciones; set { _descripcion_contribuciones = value; } }
        public string descripcion_metas { get => _descripcion_metas; set { _descripcion_metas = value; } }
        public string descripcion_participantes { get => _descripcion_participantes; set { _descripcion_participantes = value; } }
        public string color { get => _color; set { _color = value; } }
        public string fondo { get => _fondo; set { _fondo = value; } }
        public string icono { get => _icono; set { _icono = value; } }
        public double fecha_seguimiento1 { get => _fecha_seguimiento1; set { _fecha_seguimiento1 = value; } }
        public double fecha_seguimiento2 { get => _fecha_seguimiento2; set { _fecha_seguimiento2 = value; } }
        public double fecha_cierre { get => _fecha_cierre; set { _fecha_cierre = value; } }
        //public string id_attachment { get => _id_attachment; set { _id_attachment = value; } }


        public Proyecto(Row row)
        {

            _ID_estrategia = Convert.ToString(row["ID_estrategia"]);
            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _nombre = Convert.ToString(row["nombre"]);
            _descripcion = Convert.ToString(row["descripcion"]);
            _descripcion_carbono = Convert.ToString(row["descripcion_carbono"]);
            _descripcion_coberturas = Convert.ToString(row["descripcion_coberturas"]);
            _descripcion_biodiversidad = Convert.ToString(row["descripcion_biodiversidad"]);
            _descripcion_implementaciones = Convert.ToString(row["descripcion_implementaciones"]);
            _descripcion_aliados  = Convert.ToString(row["descripcion_aliados"]);
            _descripcion_contribuciones = Convert.ToString(row["descripcion_contribuciones"]);
            _descripcion_metas = Convert.ToString(row["descripcion_metas"]);
            _descripcion_participantes = Convert.ToString(row["descripcion_participantes"]);
            _color = Convert.ToString(row["color"]);
            _fondo = Convert.ToString(row["fondo"]);
            _icono = Convert.ToString(row["icono"]);
            _fecha_seguimiento1 = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_seguimiento1"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _fecha_seguimiento2 = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_seguimiento2"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _fecha_cierre = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_cierre"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            //_id_attachment = Convert.ToString(row[""]);

        }

        public Proyecto() { }
    }

    public class Aliado
    {

        private string _ID_proyecto = "";
        //private string _ID_aliado = "";
        private string _Tipo = "";
        private string _nombre = "";
        //private double _donacion= 0;
        private string _logo = "";
        private string _url = "";
        //private double _fecha;


        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        //public string ID_aliado { get => _ID_aliado; set { _ID_aliado = value; } }
        public string Tipo { get => _Tipo; set { _Tipo = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        //public double donacion { get => _donacion; set { _donacion = value; } }
        public string logo { get => _logo; set { _logo = value; } }
        public string url { get => _url; set { _url = value; } }
        //public double fecha { get => _fecha; set { _fecha = value; } }

        public Aliado(Row row)
        {

            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            //_ID_aliado = Convert.ToString(row["ID_aliado"]);
            _Tipo = Convert.ToString(row["Tipo"]);
            _nombre = Convert.ToString(row["nombre"]);
            //_donacion = Convert.ToDouble(row["donacion"]);
            _logo = Convert.ToString(row["logo"]);
            _url = Convert.ToString(row["url"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;

        }

        public Aliado() { }


    }

    public class Meta {

        private string _meta = "";
        private double _valor;
        private double _progreso;
        private string _unidad = "";
        private string _momento = "";

        public string meta { get => _meta; set { _meta = value; } }
        public double valor { get => _valor; set { _valor = value; } }
        public double progreso { get => _progreso; set { _progreso = value; } }
        public string unidad { get => _unidad; set { _unidad = value; } }
        public string momento { get => _momento; set { _momento = value; } }

        public Meta(Row row) {

            _meta = Convert.ToString(row["meta"]);
            _valor = Convert.ToDouble(row["valor"]);
            _progreso = Convert.ToDouble(row["progreso"]);
            _unidad = Convert.ToString(row["unidad"]);
            _momento = Convert.ToString(row["momento"]);

        }

        public Meta() { }
    }

    public class Participante {

        private string _ID_proyecto;
        private int _numero_hombres;
        private int _numero_mujeres;
        private int _numero_indigenas;
        private int _numero_campesinos;
        private int _numero_sin_informacion;
        private int _momento;
        //private double _fecha;

        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public int numero_hombres { get => _numero_hombres; set { _numero_hombres = value; } }
        public int numero_mujeres { get => _numero_mujeres; set { _numero_mujeres = value; } }
        public int numero_indigenas { get => _numero_indigenas; set { _numero_indigenas = value; } }
        public int numero_campesinos { get => _numero_campesinos; set { _numero_campesinos = value; } }
        public int numero_sin_informacion { get => _numero_sin_informacion; set { _numero_sin_informacion = value; } }
        public int momento { get => _momento; set { _momento = value; } }
        //public double fecha { get => _fecha; set { _fecha = value; } }

        public Participante(Row row)
        {
            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _numero_hombres = Convert.ToInt32(row["numero_hombres"]);
            _numero_mujeres = Convert.ToInt32(row["numero_mujeres"]);
            _numero_indigenas = Convert.ToInt32(row["numero_indigenas"]);
            _numero_campesinos = Convert.ToInt32(row["numero_campesinos"]);
            _numero_sin_informacion = Convert.ToInt32(row["numero_sin_informacion"]);
            _momento = Convert.ToInt32(row["momento"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds; 

        }

        public Participante() { }

    }

    public class Contribucion {

        private string _ID_proyecto;
        private string _tipo = "";
        private string _nombre = "";
        private string _logo = "";
        private string _url = "";
        private double _fecha;

        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public string tipo { get => _tipo; set { _tipo = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        public string logo { get => _logo; set { _logo = value; } }
        public string url { get => _url; set { _url = value; } }
        public double fecha { get => _fecha; set { _fecha = value; } }


        public Contribucion(Row row)
        {
            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _tipo = Convert.ToString(row["tipo"]);
            _nombre = Convert.ToString(row["nombre"]);
            _logo = Convert.ToString(row["logo"]);
            _url = Convert.ToString(row["url"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds; 

        }

        public Contribucion() { }
    }

    public class Predio
    {
        private string _ID_proyecto = "";
        private string _ID_region = "";
        private string _ID_predio = "";
        private string _nombre = "";
        private string _nombre_propietario = "";
        private string _tipo_dominio = "";
        private string _departamento = "";
        private string _municipio = "";
        private string _vereda = "";
        private double _AreaHa;
        private double _stok_carbono;
        private double _captura_carbono;
        //private double _fecha;

        public string ID_predio { get => _ID_predio; set { _ID_predio = value; } }
        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public string ID_region { get => _ID_region; set { _ID_region = value; } }
        public string nombre { get => _nombre; set { _nombre = value; } }
        public string departamento { get => _departamento; set { _departamento = value; } }
        public string municipio { get => _municipio; set { _municipio = value; } }
        public string vereda { get => _vereda; set { _vereda = value; } }
        public string nombre_propietario { get => _nombre_propietario; set { _nombre_propietario = value; } }
        public string tipo_dominio { get => _tipo_dominio; set { _tipo_dominio = value; } }
        public double AreaHa { get => _AreaHa; set { _AreaHa = value; } }
        public double stock_carbono { get => _stok_carbono; set { _stok_carbono = value; } }
        public double captura_carbono { get => _captura_carbono; set { _captura_carbono = value; } }
        //public double fecha { get => _fecha; set { _fecha = value; } }

        public Predio(Row row)
        {

            _ID_predio = Convert.ToString(row["ID_predio"]);
            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _ID_region = Convert.ToString(row["ID_region"]);
            _nombre = Convert.ToString(row["nombre"]);
            _departamento = Convert.ToString(row["departamento"]);
            _municipio = Convert.ToString(row["municipio"]);
            _vereda = Convert.ToString(row["vereda"]);
            _nombre_propietario = Convert.ToString(row["nombre_propietario"]);
            _tipo_dominio = Convert.ToString(row["tipo_dominio"]);
            _AreaHa = Convert.ToDouble(row["AreaHa"]);
            _stok_carbono = Convert.ToDouble(row["stock_carbono"]);
            _captura_carbono = Convert.ToDouble(row["captura_carbono"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;

        }

        public Predio() { }

    }

    public class Implementacion {

        private string _ID_predio = "";
        private double _area_manejo_sostenible;
        private double _area_bosque;
        private double _areas_p_sostenibles;
        private double _area_restauracion;
        private string _momento = "";
        //private double _fecha;

        public string ID_predio { get => _ID_predio; set { _ID_predio = value; } }
        public double area_manejo_sostenible { get => _area_manejo_sostenible; set { _area_manejo_sostenible = value; } }
        public double area_bosque { get => _area_bosque; set { _area_bosque = value; } }
        public double areas_p_sostenibles { get => _areas_p_sostenibles; set { _areas_p_sostenibles = value; } }
        public double area_restauracion { get => _area_restauracion; set { _area_restauracion = value; } }
        public string momento { get => _momento; set { _momento = value; } }
        //public double fecha { get => _fecha; set { _fecha = value; } }

        public Implementacion(Row row)
        {

            _ID_predio = Convert.ToString(row["ID_predio"]);
            _area_manejo_sostenible = Convert.ToDouble(row["area_manejo_sostenible"]);
            _area_bosque = Convert.ToDouble(row["area_bosque"]);
            _areas_p_sostenibles = Convert.ToDouble(row["areas_p_sostenibles"]);
            _area_restauracion = Convert.ToDouble(row["area_restauracion"]);
            _momento = Convert.ToString(row["momento"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds; 

        }

        public Implementacion() { }

    }

    public class Cobertura {

        private string _ID_predio = "";
        //private string _ID_cobertura = "";
        private string _corine1 = "";
        private string _corine2 = "";
        private string _corine3 = "";
        private string _cobertura_comun = "";
        private string _cobertura_proyecto = "";
        private string _subcobertura_proyecto = "";
        private string _uso = "";
        private string _verificacion = "";
        private double _fecha_establecimiento;
        private double _fecha_visita;
        private int _edad;
        private double _area;
        private string _momento;
        //private string _observaciones = "";
        //private double _carbono_biomasa;
       // private double _carbono_suelos;
        //private double _carbono_madera;


        public string ID_predio { get => _ID_predio; set { _ID_predio = value; } }
        //public string ID_cobertura { get => _ID_cobertura; set { _ID_cobertura = value; } }
        public string corine1 { get => _corine1; set { _corine1 = value; } }
        public string corine2 { get => _corine2; set { _corine2 = value; } }
        public string corine3 { get => _corine3; set { _corine3 = value; } }
        public string cobertura_comun { get => _cobertura_comun; set { _cobertura_comun = value; } }
        public string cobertura_proyecto { get => _cobertura_proyecto; set { _cobertura_proyecto = value; } }
        public string uso { get => _uso; set { _uso = value; } }
        public string verificacion { get => _verificacion; set { _verificacion = value; } }
        public double fecha_establecimiento { get => _fecha_establecimiento; set { _fecha_establecimiento = value; } }
        public int edad { get => _edad; set { _edad = value; } }
        public double fecha_visita { get => _fecha_visita; set { _fecha_visita = value; } }
        //public string observaciones { get => _observaciones; set { _observaciones = value; } }
        //public double carbono_biomasa { get => _carbono_biomasa; set { _carbono_biomasa = value; } }
        //public double carbono_suelos { get => _carbono_suelos; set { _carbono_suelos = value; } }
        //public double carbono_madera { get => _carbono_madera; set { _carbono_madera = value; } }
        public string subcobertura_proyecto { get => _subcobertura_proyecto; set { _subcobertura_proyecto = value; } }
        public double area { get => _area; set { _area = value; } }
        public string momento { get => _momento; set { _momento = value; } }


        public Cobertura(Row row)
        {

            _ID_predio = Convert.ToString(row["ID_predio"]);
            //_ID_cobertura = Convert.ToString(row["ID_cobertura"]);
            _corine1 = Convert.ToString(row["corine1"]);
            _corine2 = Convert.ToString(row["corine2"]);
            _corine3 = Convert.ToString(row["corine3"]);
            _cobertura_comun = Convert.ToString(row["cobertura_comun"]);
            _cobertura_proyecto = Convert.ToString(row["cobertura_proyecto"]);
            _uso = Convert.ToString(row["uso"]);
            _verificacion = Convert.ToString(row["verificacion"]);
            //_fecha_establecimiento = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_visita"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _edad = Convert.ToInt32(row["edad"]);
            _fecha_visita = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_visita"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            //_observaciones = Convert.ToString(row["observaciones"]);
            //_carbono_biomasa = Convert.ToDouble(row["carbono_biomasa"]);
            //_carbono_suelos = Convert.ToDouble(row["carbono_suelos"]);
            //_carbono_madera = Convert.ToDouble(row["carbono_madera"]);
            _subcobertura_proyecto = Convert.ToString(row["subcobertura_proyecto"]);
            _area = Convert.ToDouble(row["area"]);
            _momento = Convert.ToString(row["momento"]);

        }

        public Cobertura() { }

    }

    public class Carbono {

        private string _ID_region = "";
        private string _cobertura = "";
        private string _snc;
        private string _compartimiento = "";
        private string _sub_compartimiento = "";
        private double _T0;
        private double _T1;
        private double _T2;
        private double _T3;
        private double _T4;
        private double _T5;
        private double _T6;
        private double _T7;
        private double _T8;
        private double _T9;
        private double _T10;
        private double _T11;
        private double _T12;
        private double _T13;
        private double _T14;
        private double _T15;
        private double _T16;
        private double _T17;
        private double _T18;
        private double _T19;
        private double _T20;
        private double _fecha;
        //private string _ID_cobertura = "";


        public string cobertura { get => _cobertura; set { _cobertura = value; } }
        public double T0 { get => _T0; set { _T0 = value; } }
        public double T1 { get => _T1; set { _T1 = value; } }
        public double T2 { get => _T2; set { _T2 = value; } }
        public double T3 { get => _T3; set { _T3 = value; } }
        public double T4 { get => _T4; set { _T4 = value; } }
        public double T5 { get => _T5; set { _T5 = value; } }
        public double T6 { get => _T6; set { _T6 = value; } }
        public double T7 { get => _T7; set { _T7 = value; } }
        public double T8 { get => _T8; set { _T8 = value; } }
        public double T9 { get => _T9; set { _T9 = value; } }
        public double T10{ get => _T10; set { _T10 = value; } }
        public double T11 { get => _T11; set { _T11 = value; } }
        public double T12 { get => _T12; set { _T12 = value; } }
        public double T13 { get => _T13; set { _T13 = value; } }
        public double T14 { get => _T14; set { _T14 = value; } }
        public double T15 { get => _T15; set { _T15 = value; } }
        public double T16 { get => _T16; set { _T16 = value; } }
        public double T17 { get => _T17; set { _T17 = value; } }
        public double T18 { get => _T18; set { _T18 = value; } }
        public double T19 { get => _T19; set { _T19 = value; } }
        public double T20 { get => _T20; set { _T20 = value; } }
        public string ID_region { get => _ID_region; set { _ID_region = value; } }
        public double fecha { get => _fecha; set { _fecha = value; } }
        public string snc { get => _snc; set { _snc = value; } }
        //public string ID_cobertura { get => _ID_cobertura; set { _ID_cobertura = value; } }
        public string compartimiento { get => _compartimiento; set { _compartimiento = value; } }
        public string sub_compartimiento { get => _sub_compartimiento; set { _sub_compartimiento = value; } }

        public Carbono(Row row)
        {

            _cobertura = Convert.ToString(row["cobertura"]);
            _T0 = Convert.ToDouble(row["T0"]);
            _T1 = Convert.ToDouble(row["T1"]);
            _T2 = Convert.ToDouble(row["T2"]);
            _T3 = Convert.ToDouble(row["T3"]);
            _T4 = Convert.ToDouble(row["T4"]);
            _T5 = Convert.ToDouble(row["T5"]);
            _T6 = Convert.ToDouble(row["T6"]);
            _T7 = Convert.ToDouble(row["T7"]);
            _T8 = Convert.ToDouble(row["T8"]);
            _T9 = Convert.ToDouble(row["T9"]);
            _T10 = Convert.ToDouble(row["T10"]);
            _T11 = Convert.ToDouble(row["T11"]);
            _T12 = Convert.ToDouble(row["T12"]);
            _T13 = Convert.ToDouble(row["T13"]);
            _T14 = Convert.ToDouble(row["T14"]);
            _T15 = Convert.ToDouble(row["T15"]);
            _T16 = Convert.ToDouble(row["T16"]);
            _T17 = Convert.ToDouble(row["T17"]);
            _T18 = Convert.ToDouble(row["T18"]);
            _T19 = Convert.ToDouble(row["T19"]);
            _T20 = Convert.ToDouble(row["T20"]);
            _ID_region = Convert.ToString(row["ID_region"]);
            _fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _snc = Convert.ToString(row["SNC"]);
            //_ID_cobertura = Convert.ToString(row["ID_cobertura"]);
            _compartimiento = Convert.ToString(row["compartimiento"]);
            _sub_compartimiento = Convert.ToString(row["sub_compartimiento"]);
        }

        public Carbono() { }
    }

    public class Color {

        private string _color = "";
        private string _cobertura = "";
        //private string _ID_cobertura = "";


        public string color { get => _color; set { _color = value; } }
        public string cobertura { get => _cobertura; set { _color = value; } }
        //public string ID_cobertura { get => _ID_cobertura; set { _ID_cobertura = value; } }

        public Color(Row row)
        {

            _color = Convert.ToString(row["color"]);
            _cobertura = Convert.ToString(row["cobertura"]);
            //_ID_cobertura = Convert.ToString(row["ID_cobertura"]);

        }

        public Color() { }
    }

    public class Biodiversidad
    {

        private string _ID_region = "";
        private string _ID_proyecto = "";
        private string _ID_cobertura = "";
        private string _grupo_tnc;
        private string _cobertura = "";
        private double _fecha_identificacion;
        private int _numero_individuos;
        private string _institucion = "";
        private string _observador = "";
        private string _metodo_observacion = "";
        private string _reino = "";
        private string _filo = "";
        private string _clase = "";
        private string _orden = "";
        private string _familia = "";
        private string _genero = "";
        private string _subgenero = "";
        private string _epiteto = "";
        private string _especie = "";
        private string _autoria = "";
        private string _nombre_comun = "";
        private string _sexo = "";
        private string _momento = "";
        //private double _fecha;

        public string ID_region { get => _ID_region; set { _ID_region = value; } }
        public string ID_proyecto { get => _ID_proyecto; set { _ID_proyecto = value; } }
        public string grupo_tnc { get => _grupo_tnc; set { _grupo_tnc = value; } }
        public string cobertura { get => _cobertura; set { _cobertura = value; } }
        public double fecha_identificacion { get => _fecha_identificacion; set { _fecha_identificacion = value; } }
        public int numero_individuos { get => _numero_individuos; set { _numero_individuos = value; } }
        public string institucion { get => _institucion; set { _institucion = value; } }
        public string observador { get => _observador; set { _observador = value; } }
        public string metodo_observacion { get => _metodo_observacion; set { _metodo_observacion = value; } }
        public string reino { get => _reino; set { _reino = value; } }
        public string filo { get => _filo; set { _filo = value; } }
        public string clase { get => _clase; set { _clase = value; } }
        public string orden { get => _orden; set { _orden = value; } }
        public string familia { get => _familia; set { _familia = value; } }
        public string genero { get => _genero; set { _genero = value; } }
        public string subgenero { get => _subgenero; set { _subgenero = value; } }
        public string epiteto { get => _epiteto; set { _epiteto = value; } }
        public string sexo { get => _sexo; set { _sexo = value; } }
        public string nombre_comun { get => _nombre_comun; set { _nombre_comun = value; } }
        //public double fecha { get => _fecha; set { _fecha = value; } }
        public string especie { get => _especie; set { _especie = value; } }
        public string ID_cobertura { get => _ID_cobertura; set { _ID_cobertura = value; } }
        public string momento { get => _momento; set { _momento = value; } }

        public Biodiversidad(Row row)
        {

            _ID_region = Convert.ToString(row["ID_region"]);
            _ID_proyecto = Convert.ToString(row["ID_proyecto"]);
            _grupo_tnc = Convert.ToString(row["grupo_tnc"]);
            _cobertura = Convert.ToString(row["cobertura"]);
            _fecha_identificacion = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha_identificacion"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _numero_individuos = Convert.ToInt32(row["numero_individuos"]);
            _institucion = Convert.ToString(row["institucion"]);
            _observador = Convert.ToString(row["observador"]);
            _metodo_observacion = Convert.ToString(row["metodo_observacion"]);
            _reino = Convert.ToString(row["reino"]);
            _filo = Convert.ToString(row["filo"]);
            _clase = Convert.ToString(row["clase"]);
            _orden = Convert.ToString(row["orden"]);
            _familia = Convert.ToString(row["familia"]);
            _genero = Convert.ToString(row["genero"]);
            _subgenero = Convert.ToString(row["subgenero"]);
            _epiteto = Convert.ToString(row["epiteto"]);
            _sexo = Convert.ToString(row["sexo"]);
            _nombre_comun = Convert.ToString(row["nombre_comun"]);
            //_fecha = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row["fecha"])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            _especie = Convert.ToString(row["especie"]);
            _ID_cobertura = Convert.ToString(row["ID_cobertura"]);
            _momento = Convert.ToString(row["momento"]);

        }

        public Biodiversidad() { }
    }

    public class Carrusel
    {
        private string _Especie = "";
        private string _nombre_comun = "";
        private string _URL = "";


        public string Especie { get => _Especie; set { _Especie = value; } }
        public string nombre_comun { get => _nombre_comun; set { _nombre_comun = value; } }
        public string URL { get => _URL; set { _URL = value; } }

        public Carrusel(Row row)
        {

            _Especie = Convert.ToString(row["Especie"]);
            _nombre_comun = Convert.ToString(row["nombre_comun"]);
            _URL = Convert.ToString(row["URL"]);

        }

        public Carrusel() { }
    }
}
