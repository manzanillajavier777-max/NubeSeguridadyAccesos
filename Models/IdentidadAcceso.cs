using System.ComponentModel.DataAnnotations;
using Entities;

namespace Entities
{
    public class IdentidadAcceso
    {
        [Key]
        public int Id_Identidad {get;set;}
        public string CodigoPersona {get ; set;}
        public string TipoPersona {get;set;}
        public DateOnly FechaRegistro {get;set;}
        public string Estado {get;set;} = "Activo";
        public List<AsignacionEquipamiento> asignacionEquipamiento {get;set;}
        public List<RolIdentidad> rolPersona {get;set;}
        public List<AccesoRegistros> accesosRegistro {get;set;}
        public List<Reporte> reportes {get;set;}
        public List<DatosBiometricos> DatosBio {get;set;}
    }
}