using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly AppDbContext context;
        public RolController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListadeRolesRegistrados")]
        public async Task<IActionResult> GetRoles()
        {
            var rol = await context.Rol.ToListAsync();
            var role = rol.Select(r => r.toRolDTO()).ToList();
            return Ok(role);
            
        } 
        [HttpGet("LIstadeRolesActivos")]
        public async Task<IActionResult> GetRolesActivosDTO()
        {
            var rol = await(from ar in context.Rol
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var role = rol.Select(r => r.toRolDTO()).ToList();
            return Ok(role);
            
        } 
        [HttpPut("ActuliazarRoles")]
        public async Task<IActionResult> PutRol(string codigo,string nombre,string descripcion)
        {
            var datos = await (from ar in context.Rol
                                  where ar.CodigoRol == codigo
                                  select ar).FirstOrDefaultAsync();
            if (datos == null)
            {
                return NotFound("El Rol no fue encontrado");
            }
          datos.CodigoRol=codigo;
          datos.Nombre=nombre;
          datos.Descripcion=descripcion;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "EL Rol fue actualizado correctamente" });
        }

        [HttpPost("AnadirRol")]
        public async Task<IActionResult> PostRol(string codigo,string nombre,string descripcion)        
        {
            Rol rol = await (from pe in context.Rol
                                     where pe.CodigoRol == codigo
                                     select pe).FirstOrDefaultAsync();
                if(rol != null)
                {
                    return BadRequest("El Rol con este codigo ya existe");
                }
            Rol roll = new Rol()
            {
              CodigoRol=codigo,
              Nombre=nombre,
              Descripcion=descripcion
            };
            await context.Rol.AddAsync(roll);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El Rol fue registrado correctamente"});
        }
        [HttpDelete("EliminarRol")]
        public async Task<IActionResult> DeleteRol(string codigo)
        {
            
            var xd = await (from ar in context.Rol
                             where ar.CodigoRol == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("El Rol fue eliminado correctamente");
        }

    }
}