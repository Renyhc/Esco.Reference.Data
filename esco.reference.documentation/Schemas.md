# Schemas

Métodos:

```
	/// <summary>
    /// Devuelve el schema de trabajo actual.
    /// </summary>
    /// <returns> object result.</returns>
    public async Task<Schema> getSchema()
```
```
	/// <summary>
    /// Devuelve la lista completa de esquemas.
    /// </summary>
    /// <returns> object result.</returns>
    public async Task<Schemas> getSchemas()
```
```
    /// <summary>
    /// Devuelve un esquema con un id específico.
    /// </summary>    
	/// <param name="id">(Optional) Id del esquema. Si es null devuelve el esquema activo</param>
    /// <returns> object result.</returns>
	public async Task<Schema> getSchemaId(string id )
```
```
    /// <summary>
    /// Verifica si la tarea de promover un schema se está ejecutando
    /// </summary>
    /// <param></param>
    /// <returns> object result.</returns>
    public async Task<PromoteSchema> getPromoteSchema()
```
