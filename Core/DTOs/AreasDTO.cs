using System.Text.Json.Serialization;

namespace DTOS
{
    public class AreasDTO
    {
        [JsonPropertyName("codigo")]
        public string codigo {get;set;}

        [JsonPropertyName("nombre")]
        public string nombre {get;set;}

        [JsonPropertyName("descripcion")]
        public string descripcion {get;set;}

        [JsonPropertyName("ubicacion")]
        public string ubicacion {get;set;}

    }    
}
