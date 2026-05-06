using DTOS;
using Entities;

namespace Mapeador
{
    public static class DispositivoMapeador
    {
        public static DispositivoDTO toDispositivoDTO(this Dispositivo dis)
        {
            return new DispositivoDTO
            {
                CodigoArea = dis.CodigoArea,
                NombreDispositivo = dis.NombreDispositivo,
                NumeroSerie = dis.NumeroSerie
            };
        }
    }
}