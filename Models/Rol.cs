using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Rol
    {
        [Key]
        public int Id_Rol {get;set;}
        public string CodigoRol {get;set;}
        public string Nombre {get;set;}
        public string Descripcion {get;set;}
        public string Estado {get;set;} = "Activo";
        public List<RolIdentidad> rolIdentidad {get;set;}
        public List<RolHorario> rolHorario {get;set;}

    }
}