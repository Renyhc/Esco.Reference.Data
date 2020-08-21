using System;

namespace esco.reference.data.Services
{
    #region API
    class API
    {
        private static string _ver { get; set; }
        public static string ver
        {           
            get
            {
                return "prd-" + ((_ver == "demo") ? _ver : "ro");
            }
            set
            {
                _ver = value;
            }
        }        

        public static string v1
        {
            get { return "/v1"; }
        }

        public static string v2
        {
            get { return "/v2"; }
        }

        //Format Url with Filters
        public static string getUrl(string cfg, string type, string source, string schema)
        {
            string url = String.Format(cfg, schema);
            if ((type != null) && (source != null))
            {
                url = String.Format(cfg + Config.FilterBoth, schema, type, source);
            }
            else
            {
                url = (type != null) ? String.Format(cfg + Config.FilterType, schema, type) : url;
                url = (source != null) ? String.Format(cfg + Config.FilterSource, schema, source) : url;
            }
            return url;
        }

        public static string getUrlDerivatives(string market, string symbol)
        {
            string url = Config.Derivatives;
            string empty = String.Empty;
            string and = " and ";
            string filter = "?$filter=";

            string urlStr = (market != null) ? filter + String.Format(Config.FilterMarket, market) : empty;

            string urlSymbol = String.Format(Config.FilterSymbol, symbol);
            string urlSymbolFilter = (symbol != null) ? filter + urlSymbol : empty;
            string urlSymbolAnd = (symbol != null) ? and + urlSymbol : empty;

            urlStr = urlStr + ((urlStr == empty) ? urlSymbolFilter : urlSymbolAnd);

            return url + urlStr;
        }

        public static string getUrlFunds(string management, string despositary, string currency, string rent)
        {
            string url = Config.Funds;            
            string empty = String.Empty;
            string and = " and ";
            string filter = "?$filter=";

            string urlStr = (management != null) ? filter + String.Format(Config.FilterManagment, management) : empty;

            string urlDespositary = String.Format(Config.FilterDepositary, despositary);
            string urlDespositaryFilter = (despositary != null) ? filter  + urlDespositary : empty;
            string urlDespositaryAnd = (despositary != null) ? and + urlDespositary : empty;

            string urlCurrency = String.Format(Config.FilterCurrencyCode, currency);
            string urlCurrencyFilter = (currency != null) ? filter + urlCurrency : empty;
            string urlCurrencyAnd = (currency != null) ? and + urlCurrency : empty;

            string urlRent = String.Format(Config.FilterRent, rent);
            string urlRentFilter = (rent != null) ? filter + urlRent : empty;
            string urlRentAnd = (rent != null) ? and + urlRent : empty;

            urlStr = urlStr + ((urlStr == empty) ? urlDespositaryFilter : urlDespositaryAnd);
            urlStr = urlStr + ((urlStr == empty) ? urlCurrencyFilter : urlCurrencyAnd);
            urlStr = urlStr + ((urlStr == empty) ? urlRentFilter : urlRentAnd);
           
            return url + urlStr;
        }

        public static string getUrlOData(
            string type,
            string currency,
            string symbol,
            string market,
            string country,
            string schema)
        {
            string url = String.Format(Config.OData, schema);
            string empty = String.Empty;
            string and = " and ";
            string filter = "?$filter=";

            string urlStr = (type != null) ? filter + String.Format(Config.FilterTypeSearch, type) : empty;

            string urlCurrency = String.Format(Config.FilterCurrency, currency);
            string urlCurrencyFilter = (currency != null) ? filter + urlCurrency : empty;
            string urlCurrencyAnd = (currency != null) ? and + urlCurrency : empty;

            string urlSymbol = String.Format(Config.FilterUnderSymbol, symbol);
            string urlSymbolFilter = (symbol != null) ? filter + urlSymbol : empty;
            string urlSymbolAnd = (symbol != null) ? and + urlSymbol : empty;

            string urlMarket = String.Format(Config.FilterMarketId, market);
            string urlMarketFilter = (market != null) ? filter + urlMarket : empty;
            string urlMarketAnd = (market != null) ? and + urlMarket : empty;

            string urlCountry = String.Format(Config.FilterCountry, country);
            string urlCountryFilter = (country != null) ? filter + urlCountry : empty;
            string urlCountryAnd = (country != null) ? and + urlCountry : empty;
            
            urlStr = urlStr + ((urlStr == empty) ? urlCurrencyFilter : urlCurrencyAnd);
            urlStr = urlStr + ((urlStr == empty) ? urlSymbolFilter : urlSymbolAnd);
            urlStr = urlStr + ((urlStr == empty) ? urlMarketFilter : urlMarketAnd);
            urlStr = urlStr + ((urlStr == empty) ? urlCountryFilter : urlCountryAnd);

            return url + urlStr;
        }
    }
    #endregion

    class Config
    {   
        public static string url = "https://apids.primary.com.ar/";
        public static string cache = "no-cache";

        public class Header
        {
            public static string key = "Ocp-Apim-Subscription-Key";
            public static string cache = "Cache-Control";
            public static string xversion = "X-Version";
        }

        //Filters
        public static string FilterId           = "?$filter=indexof(id, '{1}') ne -1";
        public static string FilterType         = "?$filter=type eq {1}";
        public static string FilterTypeStr      = "?$filter=type eq '{1}'";
        public static string FilterSource       = "?$filter=source eq {1}";
        public static string FilterSourceStr    = "?$filter=source eq '{1}'";
        public static string FilterBoth         = "?$filter=type eq {1} and source eq {2}";

        public static string FilterMarket       = "indexof(marketSegmentId, '{0}') ne -1";
        public static string FilterSymbol       = "indexof(underlyingSymbol, '{0}') ne -1";   
        public static string FilterTypeSearch   = "indexof(type, '{0}') ne -1";
        public static string FilterManagment    = "indexof(managementSocietyName, '{0}') ne -1";
        public static string FilterDepositary   = "indexof(despositarySocietyName, '{0}') ne -1";        
        public static string FilterCurrencyCode = "indexof(currencyCode, '{0}') ne -1";
        public static string FilterCurrency     = "indexof(Currency, '{0}') ne -1";
        public static string FilterUnderSymbol  = "indexof(UnderlyingSymbol, '{0}') ne -1";
        public static string FilterRent         = "indexof(rentTypeName, '{0}') ne -1";
        public static string FilterMarketId     = "indexof(MarketId, '{0}') ne -1";
        public static string FilterCountry      = "indexof(Country, '{0}') ne -1";

        #region Schemas
        public static string WorkingSchema  = API.v2 + "/api/Schemas/working-schema";               //Devuelve el schema de trabajo actual.
        public static string Schemas        = API.v2 + "/api/Schemas";                              //Devuelve la lista completa de esquemas.
        public static string SchemasId      = API.v2 + "/api/Schemas/";                             //Devuelve un esquema con un id específico.
        public static string PromoteSchema  = API.v2 + "/api/Schemas/is-promote-schema-running";    //Verifica si la tarea de promover un schema se está ejecutando
        #endregion

        #region Fields
        public static string Fields = API.v2 + "/api/schema/{0}/Fields";      //Devuelve la lista completa de fields.
        public static string Field  = API.v2 + "/api/schema/{0}/Fields/{1}";  //Devuelve un field con un id específico.
        #endregion

        #region Instruments
        public static string InstrumentsSuggestedFields             = API.v2 + "/api/schema/{0}/Instruments/suggested-fields";    //Obtiene una lista de campos sugeridos.
        public static string InstrumentsTodayUpdated                = API.v2 + "/api/schema/{0}/Instruments/today-updated";       //Retorna la lista de instrumentos actualizados en el día.                
        public static string InstrumentsTodayAdded                  = API.v2 + "/api/schema/{0}/Instruments/today-added";         //Retorna la lista de instrumentos dados de alta en el día.
        public static string InstrumentsTodayRemoved                = API.v2 + "/api/schema/{0}/Instruments/today-removed";       //Retorna la lista de instrumentos dados de baja en el día.
        public static string InstrumentsReport                      = API.v2 + "/api/schema/{0}/Instruments/report";              //Retorna un reporte resumido de instrumentos.
        public static string Instrument                             = API.v2 + "/api/schema/{0}/Instruments/{1}";                 //Retorna una instrumento por id.        
        public static string Instruments                            = API.v2 + "/api/schema/{0}/Instruments";                     //Retorna una lista de instrumentos.              
        #endregion

        #region ReferenceDatas
        public static string TodayUpdated           = API.v2 + "/api/schema/{0}/ReferenceDatas/today-updated";                //Retorna la lista de instrumentos actualizados en el día.
        public static string TodayAdded             = API.v2 + "/api/schema/{0}/ReferenceDatas/today-added";                  //Retorna la lista de instrumentos dados de alta en el día.
        public static string TodayRemoved           = API.v2 + "/api/schema/{0}/ReferenceDatas/today-removed";                //Retorna la lista de instrumentos dados de baja en el día.
        public static string ReferenceDatas         = API.v2 + "/api/schema/{0}/ReferenceDatas";                              //Retorna la lista de instrumentos.        
        public static string Specification          = API.v2 + "/api/schema/{0}/ReferenceDatas/specification";                //Retorna una especificación del estado actual.
        #endregion

        #region Reports
        public static string Reports    = API.v2 + "/api/schema/{0}/Reports";                 //Devuelve la lista completa de reportes.
        public static string Report     = API.v2 + "/api/schema/{0}/Reports/{1}";             //Devuelve un reporte con un id específico.
        #endregion

        #region Types
        public static string SourceFieldTypes       = API.v2 + "/api/Types/source-field-types";       //Devuelve los posibles tipos de datos de los orígenes.
        public static string PropertyControlTypes   = API.v2 + "/api/Types/property-control-types";   //Devuelve los tipos de control de las propiedades de los instrumentos.
        public static string StateControlTypes      = API.v2 + "/api/Types/state-control-types";      //Devuelve los tipos de control del estado de un instrumento.
        public static string InstrumentTypes        = API.v2 + "/api/Types/instrument-types";         //Devuelve los tipos de instrumentos.
        public static string PropertyOriginTypes    = API.v2 + "/api/Types/property-origin-types";    //Devuelve los tipos de origen para las propiedades de los instrumentos.
        public static string SourceTypes            = API.v2 + "/api/Types/source-types";             //Devuelve los tipos de origen.        
        #endregion

        #region Mappings
        public static string Mapping    = API.v2 + "/api/schema/{0}/Mappings/{1}";            //Devuelve un mapping para un id específico.
        public static string Mappings   = API.v2 + "/api/schema/{0}/Mappings";                //Devuelve una lista de mappings.
        #endregion

        #region SourceFields
        public static string SourceFields   = API.v2 + "/api/schema/{0}/SourceFields";        //Devuelve la lista completa de source fields.
        public static string SourceField    = API.v2 + "/api/schema/{0}/SourceFields/{1}";    //Devuelve un source field con un id específico.
        #endregion

        #region StatusReports
        public static string ProcessesStatus = API.v2 + "/api/StatusReports/processes-status";    //Devuelve el estado de los procesos.
        #endregion

        #region Derivatives
        public static string Derivatives = API.v1 + "/api/Derivatives";     //Retorna una lista de derivados
        #endregion

        #region Funds
        public static string Fund           = API.v1 + "/api/Funds/{0}";    //Retorna un fondo por id        
        public static string Funds          = API.v1 + "/api/Funds";        //Retorna una lista de fondos
        #endregion       

        #region Securities
        public static string Securitie  = API.v1 + "/api/Securities/{0}";   //Retorna un titulo valor por id        
        public static string Securities = API.v1 + "/api/Securities";       //Retorna una lista de títulos valores
        #endregion 

        #region OData
        public static string OData = API.v2 + "/api/schema/{0}/refdata";    //Retorna una lista de instrumentos filtrados con OData
        #endregion 
    }
}
