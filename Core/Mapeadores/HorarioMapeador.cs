using DTOS;
using Entities;

namespace Mapeador
{
    public static class HorarioMapeador
    {
        public static HorarioDTO toHorarioDTO(this Horario hor)
        {
            return new HorarioDTO
            {
                CodigoHorario = hor.CodigoHorario,
                HoraInicio = hor.HoraInicio,
                HoraFin = hor.HoraFin
            };
        }
    }
}