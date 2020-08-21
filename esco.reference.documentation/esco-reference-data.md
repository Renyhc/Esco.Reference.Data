# Esco Reference Data

[![N|Solid](http://devops.sisesco.com/Tecnolog%C3%ADa/bf9c73ea-4fa1-4396-a9fe-1751ac976148/_apis/git/repositories/eb74888a-32cd-422a-9ede-05a685b593cf/Items?path=%2Fesco.reference.documentation%2Fesco.png)](https://www.sistemasesco.com.ar)

Conector que se integra con el Servicio [**PMYDS - Reference Data de Primary**](https://dataservices.primary.com.ar/product/#product=reference-data-read) que ofrece información de referencia de instrumentos financieros en forma consolidada.

#### DESCRIPCIÓN DE MÉTODOS

- [OData Reference Data](OData.md)
- [Reference Data](ReferenceData.md)
- [Instruments](Instruments.md)
- [Fondos](Fondos.md)
- [Schemas](Schemas)
- [Fields](Fields.md)
- [Types](Types.md)
- [Mappings](Mappings.md)
- [Source Fields](SourceFields.md)
- [Status Reports](StatusReports.md)
- [Derivatives](Derivatives.md)
- [Securities](Securities.md)

**` ReferenceDataServices`**
```sh
/// <summary>
/// Inicialización del servicio API de Reference Datas.
/// </summary>
/// <param name="key"> (Requeried) Suscription key del usuario. </param>        
/// <param name="host"> (Optional) Dirección url de la API .</param>
 
public ReferenceDataServices (string id, string host)
```

#### PROGRAMA DE TEST

- [Descargar Instalador (.zip)](reference.data.zip)

