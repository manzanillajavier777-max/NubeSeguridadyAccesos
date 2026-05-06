using DTOS;
using Entities;

namespace Mapeador
{
    public static class DatosBiometricopsMapeador
    {
        public static DatosBiometricosDTO toDatosBiometricopsDTO(this DatosBiometricos datos)
        {
            return new DatosBiometricosDTO
            {
                CodigoDato = datos.CodigoDato,
                TipoDatoBiometrico = datos.TipoDatoBiometrico,
                DatoHuella = datos.DatoHuella,
                FechaRegistro = datos.FechaRegistro
            };
        }
    }
}