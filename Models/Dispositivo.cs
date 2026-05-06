using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Dispositivo
    {
        [Key]
        public int Id_Dispositivo {get;set;}
        public string CodigoArea {get;set;}
        public string NombreDispositivo {get;set;}
        public string NumeroSerie {get;set;}
        public string Estado {get;set;} = "Activo";
        public List<AccesoRegistros> accesoRegistro {get;set;}
    }
}