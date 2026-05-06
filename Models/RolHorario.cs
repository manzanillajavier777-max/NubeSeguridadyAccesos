using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class RolHorario
    {
        [Key]
        public int Id {get;set;}
        public string codigoArea {get;set;}
        public int rolId {get;set;}
        public int HoraioId {get;set;}
        
        [ForeignKey("Id_Horario")]
        [JsonIgnore]
        public Horario horario {get;set;}
        [ForeignKey("Id_Rol")]
        [JsonIgnore]
        public Rol rol {get;set;}
    }
}