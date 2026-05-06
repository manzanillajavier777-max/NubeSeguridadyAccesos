using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class AccesoRegistros
    {
        [Key]
        public int id_Registros {get;set;}
        public int IdentidadId {get;set;}
        public int DispositivoId {get;set;}
        public string CodigoArea {get;set;}
        public DateOnly FechaRegistro {get;set;}
        public DateTime HorarioRegistro {get;set;}
        public string Descripcion {get;set;}
        public string Estado {get;set;} = "Activo";
        [ForeignKey("Id_Identidad")]
        [JsonIgnore]
        public IdentidadAcceso identidad {get;set;}
        [ForeignKey("Id_Dispositivo")]
        [JsonIgnore]
        public Dispositivo dispositivo {get;set;}
    }
}