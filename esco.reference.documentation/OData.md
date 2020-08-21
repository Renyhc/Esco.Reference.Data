# OData ReferenceDatas

Métodos:

**` getODataReferenceDatas`**
```
	/// <summary>
    /// Retorna la lista de instrumentos financieros filtrados con Query en formato ODat.
    /// </summary>
    /// <param name="query">(Optional) Query de filtrado en formato OData. Diccionario de campos disponible con el método getReferenceDataSpecification("2"). (Ejemplo de consulta:"?$top=5&$filter=type eq 'MF'&$select=Currency,Symbol,UnderlyingSymbol" </param>
    /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
    /// <returns>ReferenceDatas object Result.</returns>
    public async Task<ODataObject> getODataReferenceDatas(string query = null, string schema = null)
```

**` getODataReferenceDatasById`**
```
    /// <summary>
    /// Retorna la lista de instrumentos financieros filtrados por Id.
    /// </summary>
    /// <param name="id">(Requeried) Cadena de búsqueda de los Instrumentos por Id.</param>
    /// <param name="schema">(Optional) Id del esquema de devolución de la información. Si es null se toma por defecto el esquema activo.</param>
    /// <returns>OData object Result.</returns>
    public async Task<ODataReferenceDatas> getODataReferenceDatasById(string id, string schema = null)
```

**` searchODataReferenceDatas`**
```
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
    public async Task<ODataReferenceDatas> searchODataReferenceDatas(
            string type = null, 
            string currency = null, 
            string symbol = null, 
            string market = null, 
            string country = null, 
            string schema = null)
```