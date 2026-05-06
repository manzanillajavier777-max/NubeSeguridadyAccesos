using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class DatosBiometricos
    {
        [Key]
        public int Id_DatosBio {get;set;}
        public string CodigoDato {get;set;}
        public string TipoDatoBiometrico {get;set;}
        public string DatoHuella {get;set;}
        public DateOnly FechaRegistro {get;set;}
        public string Estado {get;set;} = "Activo";
        public int IdentidadId {get;set;}
        [ForeignKey("Id_Identidad")]
        [JsonIgnore]
        public IdentidadAcceso identiidad {get;set;}

    }
}