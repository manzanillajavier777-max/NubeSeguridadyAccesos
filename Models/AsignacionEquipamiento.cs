using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Entities
{
    public class AsignacionEquipamiento
    {
        public int Id {get;set;}
        public int identidadId {get;set;}
        public int equipamientoId {get;set;}
        public DateOnly fechaEntrega {get;set;}
        public string Estado {get;set;} = "Activo";
        [ForeignKey("Id_Identidad")]
        [JsonIgnore]
        public IdentidadAcceso identidad {get;set;}
        [ForeignKey("Id_Equipamiento")]
        [JsonIgnore]
        public Equipamiento equipamiento {get;set;}
    }
}