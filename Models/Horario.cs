using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Entities
{
    public class Horario
    {
        [Key]
        public int Id_Horario {get;set;}
        public string CodigoHorario {get;set;}
        public string HoraInicio {get;set;}
        public string HoraFin {get;set;}
        public string Estado {get;set;} = "Activo";
        public List<RolHorario> rolHorarios {get;set;}
    }
}