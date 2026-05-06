namespace DTOS
{
    public class DatosBiometricosDTO
    {
        public string CodigoDato {get;set;}
        public string TipoDatoBiometrico {get;set;}
        public string DatoHuella {get;set;}
        public DateOnly FechaRegistro {get;set;}

    }
}