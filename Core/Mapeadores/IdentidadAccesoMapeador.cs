using DTOS;
using Entities;

namespace Mapeador
{
    public static class IdentidadAccesoMapeador
    {
        public static IdentidadAccesoDTO toIdentidadAccesoDTO(this IdentidadAcceso ide)
        {
            return new IdentidadAccesoDTO
            {
                CodigoPersona = ide.CodigoPersona,
                TipoPersona = ide.TipoPersona,
                FechaRegistro = ide.FechaRegistro,
            };
        }
    }
}