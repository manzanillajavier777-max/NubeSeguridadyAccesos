using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolIdentidadController : ControllerBase
    {
        private readonly AppDbContext context;
        public RolIdentidadController(AppDbContext context)
        {
            this.context = context; 
        }
        [HttpGet("QueryListarTodosLosRolesAsignados")]
        public async Task<IActionResult> GetListarTodosLosRolesAsignados()
        {
            var query = await(from rol in context.Rol
                              join ri in context.RolIdentidad on rol.Id_Rol  equals ri.RolId
                              join ide in context.IdentidadAcceso on ri.IdentidadId  equals ide.Id_Identidad
                              select new
                              {
                                  CodigoPersona = ide.CodigoPersona,
                                  TipoDePersona = ide.TipoPersona,
                                  Rol = rol.Nombre,
                                  Descripcion = rol.Descripcion
                              }
                              ).ToListAsync();
            return Ok(query);
        }
        [HttpGet("QueryListarRolesAsignadosACadaPersona")]
        public async Task<IActionResult> GetListarRolesAsignadosACadaPersona(string CodigoIdentidad)
        {
            var query = await(from rol in context.Rol
                              join ri in context.RolIdentidad on rol.Id_Rol  equals ri.RolId
                              join ide in context.IdentidadAcceso on ri.IdentidadId  equals ide.Id_Identidad
                              where ide.CodigoPersona == CodigoIdentidad
                              select new
                              {
                                  CodigoPersona = ide.CodigoPersona,
                                  Rol = rol.Nombre
                              }
                              ).ToListAsync();
            return Ok(query);
        }
        [HttpPost("AgregarRol")]
        public async Task<IActionResult> PostAgregarRol(string codigorol,string codigoidentidad)
        {
            IdentidadAcceso? identidad = await (from id in context.IdentidadAcceso
                                              where id.CodigoPersona == codigoidentidad
                                              select id).FirstOrDefaultAsync();

            Rol? rol = await (from role in context.Rol
                            where role.CodigoRol == codigorol
                            select role).FirstOrDefaultAsync();
            if(identidad == null || rol == null)
            {
                return BadRequest("La identidad o Rol no existe");
            }
            var rolidentidad =await (from rh in context.RolIdentidad
                                    where rh.IdentidadId == identidad.Id_Identidad && rh.RolId == rol.Id_Rol
                                    select rh).FirstOrDefaultAsync();
            if(rolidentidad != null)
            {
                return BadRequest("La relacion ya existe");
            }
            var rolide = new RolIdentidad
            {
                RolId =rol.Id_Rol,
                IdentidadId = identidad.Id_Identidad,
                rol = rol,
                identidad =identidad
                
            };
            context.RolIdentidad.Add(rolide);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="Rol Registrado Correctamente"});
        }
    }
}