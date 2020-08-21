using System;
using System.Linq;
using System.Threading.Tasks;
using esco.reference.data.Model;
using esco.reference.data.Services.Contracts;
using Newtonsoft.Json;

namespace esco.reference.data.Services
{
    /// <summary>
    /// Servicios Reference Datas Conector que se integra con el Servicio PMYDS - Reference Data de Primary .
    /// </summary>   
    public class ReferenceDataServices : IReferenceDataServices
    {
        private ReferenceDataHttpClient _httpClient;

        /// <summary>
        /// Inicialización del servicio API de Reference Datas.
        /// </summary>
        /// <param name="key">(Required) Suscription key del usuario. Requerido para poder operar en la API (Solicitar habilitación de la suscripción despues de la creación de cuenta).</param>                
        /// <param name="version">(Optional) Definir version demo o de producción de la Api (parametros aceptados: "demo", "ro", si es null toma la version de producción por defecto).</param>                
        /// <param name="host">(Optional) Dirección url de la API Reference Data. Si es null toma el valor por defecto: https://apids.primary.com.ar/ </param>
        /// <returns></returns>
        public ReferenceDataServices(string key, string version = null, string host = null)
        {
            _httpClient = new ReferenceDataHttpClient(key, version, host);
        }

        /// <summary>
        /// Cambiar la Suscription Key del usuario.
        /// </summary>
        /// <param name="key">(Required) Suscription key del usuario. Requerido para poder operar en la API (Solicitar habilitación de la suscripción despues de la creación de cuenta).</param>                        
        /// <returns></returns>
        public void changeSuscriptionKey(string key)
        {
            _httpClient.changeKey(key);
        }

        private async Task<string> getSchemaActive()
        {
            Schema _schema = await _httpClient.getSchema();
            return _schema.id;
        }

        private async Task<string> getTypes(string source)
        {
            Types sourceType = await _httpClient.getSourceTypes();
            TypeField types = sourceType.Where(s => s.description == source).FirstOrDefault();
            return types?.code.ToString();           
        }

        #region Schemas
        /// <summary>
        /// Devuelve el schema de trabajo actual.
        /// </summary>
        /// <param></param>
        /// <returns>Schema object result</returns>
        public async Task<Schema> getSchema()
        {            
            try
            {
                return await _httpClient.getSchema();               
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve la lista completa de esquemas.
        /// </summary>
        /// <param></param>
        /// <returns>Schemas object Result.</returns>
        public async Task<Schemas> getSchemas()
        {
            try
            {
                return await _httpClient.getSchemas();
            }
            catch (Exception e)
            {
                throw e;
            }           
        }

        /// <summary>
        /// Devuelve un esquema con un id específico.
        /// </summary>
        /// <param name="id">(Optional) Id del esquema. Si es null devuelve el esquema activo</param>
        /// <returns>Schema object Result.</returns>
        public async Task<Schema> getSchemaId(string id = null)
        {            
            try
            {
                if (id == null)
                {
                    Schema schema = await _httpClient.getSchema();
                    id = (schema != null)? schema.id: "0";
                }

                return await _httpClient.getSchemaId(id);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Verifica si la tarea de promover un schema se está ejecutando
        /// </summary>
        /// <param></param>
        /// <returns>PromoteSchema object Result.</returns>
        public async Task<PromoteSchema> getPromoteSchema()
        {            
            try
            {
                return await _httpClient.getPromoteSchema();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        #endregion       

        #region Fields
        /// <summary>
        /// Devuelve la lista completa de fields.
        /// </summary>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>FieldsList object Result.</returns>
        public async Task<FieldsList> getFields(string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getFields(schema);             
            }
            catch (Exception e)
            {           
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve un field con un id específico.
        /// </summary>
        /// <param name="id">(Required) Id del Field a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Field object Result.</returns>
        public async Task<Field> getField(string id, string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getField(id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }
        #endregion

        #region Instruments

        /// <summary>
        /// Obtiene una lista de campos sugeridos.
        /// </summary>        
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>SuggestedFields object Result.</returns>
        public async Task<SuggestedFields> getInstrumentsSuggestedFields(string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getInstrumentsSuggestedFields(schema);                
            }
            catch (Exception e)
            {               
                throw e;
            }            
        }

        /// <summary>
        /// Retorna la lista de instrumentos actualizados en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="source">(Optional) Filtrar por mercado (source). Valores permitidos: "ROFEX", "CAFCI", "BYMA". Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> getInstrumentsTodayUpdated(string type = null, string source = null, string schema = null)
        {            
            return await _httpGetInstruments(Config.InstrumentsTodayUpdated, type, source, schema);
        }

        /// <summary>
        /// Retorna los instrumentos actualizados en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos actualizados en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> searchInstrumentsTodayUpdated(string id, string schema = null)
        {
            return await _httpSearchInstruments(Config.InstrumentsTodayUpdated, id, schema);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de alta en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="source">(Optional) Filtrar por mercado (source). Valores permitidos: "ROFEX", "CAFCI", "BYMA". Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> getInstrumentsTodayAdded(string type = null, string source = null, string schema = null)
        {            
            return await _httpGetInstruments(Config.InstrumentsTodayAdded, type, source, schema);
        }

        /// <summary>
        /// Retorna los instrumentos dados de alta en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos dados de alta en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> searchInstrumentsTodayAdded(string id, string schema = null)
        {
            return await _httpSearchInstruments(Config.InstrumentsTodayAdded, id, schema);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de baja en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="source">(Optional) Filtrar por mercado (source). Valores permitidos: "ROFEX", "CAFCI", "BYMA". Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> getInstrumentsTodayRemoved(string type = null, string source = null, string schema = null)
        {            
            return await _httpGetInstruments(Config.InstrumentsTodayRemoved, type, source, schema);
        }

        /// <summary>
        /// Retorna los instrumentos dados de baja en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos dados de baja en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> searchInstrumentsTodayRemoved(string id, string schema = null)
        {
            return await _httpSearchInstruments(Config.InstrumentsTodayRemoved, id, schema);
        }

        /// <summary>
        /// Retorna un reporte resumido de instrumentos.
        /// </summary>
        /// <param name="source">(Optional) Filtrar por tipo de mercado (source). Valores permitidos: "ROFEX", "CAFCI", "BYMA". Si es null devuelve la lista completa.</param>      
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>InstrumentsReport object Result.</returns>
        public async Task<InstrumentsReport> getInstrumentsReport(string source = null, string schema = null)
        {
            Response result = new Response();
            try
            {
                schema = (schema == null) ? await getSchemaActive() : schema;
                return await _httpClient.getInstrumentsReport(source, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna los instrumentos del reporte resumido contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">">(Requeried) Cadena de búsqueda de los Instrumentos del reporte resumido a filtrar</param>      
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>InstrumentsReport object Result.</returns>
        public async Task<InstrumentsReport> searchInstrumentsReport(string id, string schema = null)
        {            
            try
            {
               schema = (schema == null) ? await getSchemaActive() : schema;
               return await _httpClient.searchInstrumentsReport(id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Retorna una instrumento por id.
        /// </summary>
        /// <param name="id">(Requeried) Id del Instrumento a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instrument object Result.</returns>
        public async Task<Instrument> getInstrument(string id, string schema = null)
        {
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getInstrument(id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna los instrumentos que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> searchInstruments(string id, string schema = null)
        {
            return await _httpSearchInstruments(Config.Instruments, id, schema);
        }

        /// <summary>
        /// Retorna una lista de instrumentos.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve todos los tipos de Instrumentos.</param>
        /// <param name="source">(Optional) Filtrar por mercado (source). Valores permitidos: "ROFEX", "CAFCI", "BYMA". Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Instruments object Result.</returns>
        public async Task<Instruments> getInstruments(string type = null, string source = null, string schema = null)
        {
            return await _httpGetInstruments(Config.Instruments, type, source, schema);
        }

        private async Task<Instruments> _httpGetInstruments(string cfg, string type, string source, string schema)
        {            
            try
            {
                schema = (schema == null) ? await getSchemaActive() : schema; 
                source = (source != null) ? await getTypes(source) : source;
                return await _httpClient.getInstruments(cfg, type, source, schema);                
            }
            catch (Exception e)
            {               
                throw e;
            }            
        }

        private async Task<Instruments> _httpSearchInstruments(string cfg, string id, string schema)
        {            
            try
            {
                schema = (schema == null) ? await getSchemaActive() : schema;
                return await _httpClient.searchInstruments(cfg, id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        #endregion

        #region ReferenceDatas        

        /// <summary>
        /// Retorna la lista de instrumentos actualizados en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> getReferenceDataTodayUpdated(string type = null, string schema = null)
        {            
            return await _httpReferenceDatas(Config.TodayUpdated, type, schema, false);
        }

        /// <summary>
        /// Retorna la lista de instrumentos actualizados en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos actualizados en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> searchReferenceDataTodayUpdated(string id, string schema = null)
        {
            return await _httpReferenceDatas(Config.TodayUpdated, id, schema, true);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de alta en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> getReferenceDataTodayAdded(string type = null, string schema = null)
        {            
            return await _httpReferenceDatas(Config.TodayAdded, type, schema, false);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de alta en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos dados de alta en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> searchReferenceDataTodayAdded(string id, string schema = null)
        {
            return await _httpReferenceDatas(Config.TodayAdded, id, schema, true);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de baja en el día.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> getReferenceDataTodayRemoved(string type = null, string schema = null)
        {
            return await _httpReferenceDatas(Config.TodayRemoved, type, schema, false);
        }

        /// <summary>
        /// Retorna la lista de instrumentos dados de baja en el día que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos dados de baja en el día a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> searchReferenceDataTodayRemoved(string id, string schema = null)
        {
            return await _httpReferenceDatas(Config.TodayRemoved, id, schema, true);
        }

        /// <summary>
        /// Retorna la lista de instrumentos financieros.
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos. Si es null devuelve la lista completa.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>        
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> getReferenceDatas(string type = null, string schema = null)
        {            
            return await _httpReferenceDatas(Config.ReferenceDatas, type, schema, false);
        }

        /// <summary>
        /// Retorna los Instrumentos financieros que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos financieros a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>        
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ReferenceDatas> searchReferenceDatas(string id, string schema = null)
        {           
            return await _httpReferenceDatas(Config.ReferenceDatas, id, schema, true);
        }

        private async Task<ReferenceDatas> _httpReferenceDatas(string cfg, string str, string schema, bool search)
        {
            try
            {
                schema = (schema == null) ? await getSchemaActive() : schema;
                return (search) ?
                    await _httpClient.searchReferenceDatas(cfg, str, schema) :
                    await _httpClient.getReferenceDatas(cfg, str, schema);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retorna una especificación del estado actual.
        /// </summary>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Specification object Result.</returns>
        public async Task<Specification> getReferenceDataSpecification(string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getSpecification(schema);               
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        #endregion

        #region Reports        
        /// <summary>
        /// Devuelve la lista completa de reportes.
        /// </summary>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Reports object Result.</returns>
        public async Task<Reports> getReports(string schema = null)
        {
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getReports(schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve un reporte con un id específico.
        /// </summary>
        /// <param name="id">(Requeried) Id del Reporte a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Report object Result.</returns>
        public async Task<Report> getReport(string id, string schema = null)
        {
            try
            {
                schema = (schema == null) ? await getSchemaActive() : schema;
                return await _httpClient.getReport(id,schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }
        #endregion

        #region Types

        /// <summary>
        /// Devuelve los posibles tipos de datos de los orígenes.
        /// </summary>
        /// <returns>Types object Result.</returns>
        public async Task<Types> getSourceFieldTypes()
        {            
            try
            {
                return await _httpClient.getSourceFieldTypes();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve los tipos de control de las propiedades de los instrumentos
        /// </summary>       
        /// <returns>Types object Result.</returns>
        public async Task<Types> getPropertyControlTypes()
        {            
            try
            {
                return await _httpClient.getPropertyControlTypes();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve los tipos de control del estado de un instrumento
        /// </summary>       
        /// <returns>Types object Result.</returns>
        public async Task<Types> getStateControlTypes()
        {            
            try
            {
               return await _httpClient.getStateControlTypes();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve los tipos de instrumentos
        /// </summary>       
        /// <returns>Types object Result.</returns>
        public async Task<Types> getInstrumentTypes()
        {            
            try
            {
               return await _httpClient.getInstrumentTypes();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve los tipos de origen para las propiedades de los instrumentos
        /// </summary>       
        /// <returns>object Result.</returns>
        public async Task<Types> getPropertyOriginTypes()
        {            
            try
            {
                return await _httpClient.getPropertyOriginTypes();              
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve los tipos de origen
        /// </summary>       
        /// <returns>Types object Result.</returns>
        public async Task<Types> getSourceTypes()
        {            
            try
            {
                return await _httpClient.getSourceTypes();         
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        #endregion

        #region Mappings        
        /// <summary>
        /// Devuelve un mapping para un id específico.
        /// </summary>
        /// <param name="id">(Requeried) Id del Mapping a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Mapping object Result.</returns>
        public async Task<Mapping> getMapping(string id = null, string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getMapping(id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve una lista de mappings.
        /// </summary>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>Mappings object Result.</returns>
        public async Task<Mappings> getMappings(string schema = null)
        {
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getMappings(schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        #endregion

        #region SourceFields        
        /// <summary>
        /// Devuelve la lista completa de source fields.
        /// </summary>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>SourceFields object Result.</returns>
        public async Task<SourceFields> getSourceFields(string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getSourceFields(schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Devuelve un source field con un id específico.
        /// </summary>
        /// <param name="id">(Requeried) Id del Source Field a filtrar.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>SourceField object Result.</returns>
        public async Task<SourceField> getSourceField(string id = null, string schema = null)
        {            
            try
            {
                schema = (schema == null)? await getSchemaActive(): schema;
                return await _httpClient.getSourceField(id, schema);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }
        #endregion

        #region StatusReports        
        /// <summary>
        /// Devuelve el estado de los procesos.
        /// </summary>
        /// <returns>Status object Result.</returns>
        public async Task<Status> getStatusProcesses()
        {            
            try
            {
                return await _httpClient.getStatusProcesses();                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Derivatives        
        /// <summary>
        /// Retorna una lista de derivados
        /// </summary>
        /// <param name="marketSegmentId">(Optional) Id del segmento de mercado (Ej: "DDA", "MATBA", puede incluirse una cadena de búsqueda parcial)</param>
        /// <param name="underlyingSymbol">(Optional) Símbolo del Derivado (Ej: "Indice Novillo Pesos", puede incluirse una cadena de búsqueda parcial).</param>
        /// <returns>Derivatives object Result.</returns>
        public async Task<Derivatives> getDerivatives(string marketSegmentId = null, string underlyingSymbol = null)
        {           
            try
            {
                return await _httpClient.getDerivatives(marketSegmentId, underlyingSymbol);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna una lista de derivados que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Derivadis a filtrar.</param>        
        /// <returns>Instruments object Result.</returns>
        public async Task<Derivatives> searchDerivatives(string id)
        {
            try
            {
                return await _httpClient.searchDerivatives(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Funds        
        /// <summary>
        /// Retorna un fondo por id
        /// </summary>
        /// <param name="id">(Requeried) Id del Fondo a filtrar.</param>
        /// <returns>Fund object Result.</returns>
        public async Task<Fund> getFund(string id)
        {            
            try
            {
                return await _httpClient.getFund(id);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna una lista de fondos filtrado por campos específicos
        /// </summary>
        /// <param name="managment">(Optional) Filtar por Sociedad Administración (puede incluirse una cadena de búsqueda parcial)</param>
        /// <param name="depositary">(Optional) Filtar por Sociedad Depositoria (puede incluirse una cadena de búsqueda parcial)</param>
        /// <param name="currency">(Optional) Filtar por Moneda (Ejemplo: "ARS", "USD")</param>
        /// <param name="rentType">(Optional) Filtar por Tipo de Renta (puede incluirse una cadena de búsqueda parcial)</param>
        /// <returns>Funds object Result.</returns>
        public async Task<Funds> getFunds(
            string managment = null, 
            string depositary = null, 
            string currency = null, 
            string rentType = null)
        {            
            try
            {
                return await _httpClient.getFunds(managment, depositary, currency, rentType);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna una lista de fondos que contengan una cadena de búsqueda como parte del id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Fondos a filtrar. Si es null devuelve todos los Fondos</param>
        /// <returns>Funds object Result.</returns>
        public async Task<Funds> searchFunds(string id)
        {
            try
            {
                return await _httpClient.searchFunds(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion      

        #region Securities        
        /// <summary>
        /// Retorna un titulo valor por id
        /// </summary>
        /// <param name="id">(Requeried) Id del título valor a filtrar.</param>
        /// <returns>Securitie object Result.</returns>
        public async Task<Securitie> getSecuritie(string id)
        {            
            try
            {
                return await _httpClient.getSecuritie(id);         
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        /// <summary>
        /// Retorna una lista de títulos valores
        /// </summary>
        /// <param name="id">(Optional) Cadena de búsqueda de los títulos valores a filtrar. Si es null devuelve todos los títulos valores</param>
        /// <returns>Securities object Result.</returns>
        public async Task<Securities> getSecurities(string id = null)
        {            
            try
            {
                return await _httpClient.getSecurities(id);                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }
        #endregion

        #region OData

        /// <summary>
        /// Retorna la lista de instrumentos financieros filtrados con Query en formato OData.
        /// </summary>
        /// <param name="query">(Optional) Query de filtrado en formato OData. Diccionario de campos disponible con el método getReferenceDataSpecification("2"). (Ejemplo de consulta:"?$top=5&$filter=type eq 'MF'&$select=Currency,Symbol,UnderlyingSymbol" </param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ODataObject> getODataReferenceDatas(string query = null, string schema = null)
        {
            try
            {
                schema = (schema == null) ? "2" : schema;
                query = (query != null) ? query : String.Empty;
                return await _httpClient.getODataReferenceDatas(query, schema);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retorna la lista de instrumentos financieros filtrados por Id.
        /// </summary>
        /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos por Id.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
        /// <returns>OData object Result.</returns>
        public async Task<ODataObject> getODataReferenceDatasById(string id, string schema = null)
        {
            try
            {
                schema = (schema == null) ? "2" : schema;                
                return await _httpClient.getODataReferenceDatasById(id, schema);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retorna la lista de instrumentos financieros filtrados por campos específicos (puede incluirse cadenas de búsqueda parcial).
        /// </summary>
        /// <param name="type">(Optional) Filtrar por tipo de Instrumentos.</param>
        /// <param name="currency">(Optional) Filtrar por tipo de Moneda.</param>
        /// <param name="symbol">(Optional) Filtrar por símbolo de Instrumentos.</param>        
        /// <param name="market">(Optional) Filtrar por Tipo de Mercado.</param>
        /// <param name="country">(Optional) Filtrar por País.</param>
        /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema "2".</param>
        /// <returns>ReferenceDatas object Result.</returns>
        public async Task<ODataObject> searchODataReferenceDatas(
            string type = null, 
            string currency = null, 
            string symbol = null, 
            string market = null, 
            string country = null, 
            string schema = null)
        {
            try
            {
                schema = (schema == null) ? "2" : schema;
                return await _httpClient.searchODataReferenceDatas(
                    type, 
                    currency, 
                    symbol, 
                    market, 
                    country, 
                    schema);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        #endregion 
    }
}