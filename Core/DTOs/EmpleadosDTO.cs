using System.Text.Json.Serialization;

namespace DTOS
{
    public class EmpleadosDto
    {
        [JsonPropertyName("codigoEmpleado")]
        public string codigoEmpleado {get;set;}
        [JsonPropertyName("ci")]
        public string ci {get;set;}
        [JsonPropertyName("nombre")]
        public string nombre {get;set;}
        [JsonPropertyName("apellido")]
        public string apellido {get;set;}
        [JsonPropertyName("fechaContratacion")]
        public DateTime fechaContratacion {get;set;}
        [JsonPropertyName("salarioBase")]
        public int salarioBase {get;set;}

    }    
}
