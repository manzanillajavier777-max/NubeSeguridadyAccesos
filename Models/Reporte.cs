using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Reporte
    {
        [Key]
        public int Id_Reporte {get;set;}
        public string codigoReporte {get;set;}
        public string TipoReporte {get;set;}
        public string Descripcion {get;set;}
        public DateTime FechaReporte {get;set;}
        public string Estado {get;set;} = "Activo";
        public int IdentidadId {get;set;}
        
        [ForeignKey("Id_Identidad")]
        [JsonIgnore]
        public IdentidadAcceso identidadd {get;set;}
    }
}