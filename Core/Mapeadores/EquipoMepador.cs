using DTOS;
using Entities;

namespace Mapeador
{
    public static class EquipoMapeador
    {
        public static EquipamientoDTO toEquipamientoDTO(this Equipamiento equi)
        {
            return new EquipamientoDTO
            {
                Nombre_Equipo = equi.Nombre_Equipo,
                CodigoEquipamiento = equi.CodigoEquipamiento,
                Descripcion = equi.Descripcion
            };
        }
    }
}