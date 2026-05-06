using DTOS;
using Entities;

namespace Mapeador
{
    public static class RolMapeador
    {
        public static RolDTO toRolDTO(this Rol rol)
        {
            return new RolDTO
            {
                CodigoRol = rol.CodigoRol,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            };
        }
    }
}