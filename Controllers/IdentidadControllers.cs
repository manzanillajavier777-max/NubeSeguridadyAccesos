using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentidadAcessoController : ControllerBase
    {
        private readonly AppDbContext context;
        public IdentidadAcessoController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListadePersonasResgistradasDTO")]
        public async Task<IActionResult> GetIdentidad()
        {
            var ide = await context.IdentidadAcceso.ToListAsync();
            var identidad = ide.Select(i => i.toIdentidadAccesoDTO()).ToList();
            return Ok(identidad);
            
        } 
        [HttpGet("LIstadePersonaActivasDTO")]
        public async Task<IActionResult> GetIdentidadActivos()
        {
            var ide = await(from ar in context.IdentidadAcceso
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var identidad = ide.Select(i => i.toIdentidadAccesoDTO()).ToList();
            return Ok(identidad);
            
        } 
        [HttpGet("MostrarSegunElCodigoDTO")]
        public async Task<IActionResult> GetIdentidadCodigo(string codigo)
        {
            var ide =  await(from ar in context.IdentidadAcceso
                           where ar.CodigoPersona == codigo
                           select ar).ToListAsync();
            var identidad = ide.Select(i => i.toIdentidadAccesoDTO()).ToList();
            if (!identidad.Any())
            {
                return BadRequest("La Persona no existe");
            }
            return Ok(identidad);
        }
        [HttpPut("ActuliazarIdentidad")]
        public async Task<IActionResult> PutIdentidad(string codigo, string TipoPersona)
        {
            var Identidad = await (from ar in context.IdentidadAcceso
                                  where ar.CodigoPersona == codigo
                                  select ar).FirstOrDefaultAsync();
            if (Identidad == null)
            {
                return NotFound("La persona no fue encontrada");
            }
            Identidad.CodigoPersona=codigo;
            Identidad.TipoPersona = TipoPersona;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "La persona fue actualizada correctamente" });
        }

        [HttpPost("AnadirIdentidad")]
        public async Task<IActionResult> PostIdentidad(string codigo,string TipoPersona,DateOnly fecharegistro )        
        {
            IdentidadAcceso identidad = await (from pe in context.IdentidadAcceso
                                     where pe.CodigoPersona == codigo
                                     select pe).FirstOrDefaultAsync();
                if(identidad != null)
                {
                    return BadRequest("La persona con este codigo ya existe");
                }
            IdentidadAcceso acceso = new IdentidadAcceso()
            {
              CodigoPersona = codigo,
              TipoPersona=TipoPersona,
              FechaRegistro=fecharegistro  
            };
            await context.IdentidadAcceso.AddAsync(acceso);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="La persona fue registrada correctamente"});
        }
        [HttpDelete("EliminarIdentidad")]
        public async Task<IActionResult> DeleteIdentidad(string codigo)
        {
            
            var xd = await (from ar in context.IdentidadAcceso
                             where ar.CodigoPersona == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("Persona Eliminada correctamente");
        }

    }
}