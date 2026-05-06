using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Equipamiento
    {
        [Key]
        public int Id_Equipamiento {get;set;}
        public string Nombre_Equipo{get;set;}
        public string CodigoEquipamiento {get;set;}
        public string Descripcion {get;set;}
        public string Estado {get;set;} = "Activo";
        public List<AsignacionEquipamiento> AsignacionEquipamietnos {get;set;}
    }
}