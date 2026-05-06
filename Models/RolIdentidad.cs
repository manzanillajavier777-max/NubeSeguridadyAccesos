using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class RolIdentidad
    {
        [Key]
        public int Id {get;set;}
        public int RolId {get;set;}
        public int IdentidadId {get;set;}
        
        [ForeignKey("Id_Identidad")]
        [JsonIgnore]
        public IdentidadAcceso identidad {get;set;}
        [ForeignKey("Id_Rol")]
        [JsonIgnore]
        public Rol rol {get;set;}
    }
}