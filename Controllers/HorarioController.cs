using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly AppDbContext context;
        public HorarioController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListadeHorariosegistradosDTO")]
        public async Task<IActionResult> GetHorario()
        {
            var hor = await context.Horario.ToListAsync();
            var horario =hor.Select( h => h.toHorarioDTO()).ToList();
            return Ok(horario);
            
        } 
        [HttpGet("LIstadeHorariosActivosDTO")]
        public async Task<IActionResult> GetHorariosActivos()
        {
            var hor = await(from ar in context.Horario
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var horario =hor.Select( h => h.toHorarioDTO()).ToList();
            return Ok(horario);
            
        } 
        [HttpPut("ActuliazarHorario")]
        public async Task<IActionResult> PutDatosBio(string codigo, string HorarioInicio,string HoraFin)
        {
            var datos = await (from ar in context.Horario
                                  where ar.CodigoHorario == codigo
                                  select ar).FirstOrDefaultAsync();
            if (datos == null)
            {
                return NotFound("El Horario no fue encontrado");
            }
           datos.CodigoHorario=codigo;
           datos.HoraInicio=HorarioInicio;
           datos.HoraFin=HoraFin;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "EL Horariofue actualizado correctamente" });
        }

        [HttpPost("AnadirHorario")]
        public async Task<IActionResult> PostDatos(string codigo, string HorarioInicio,string HoraFin )        
        {
            Horario? dis = await (from pe in context.Horario
                                     where pe.CodigoHorario == codigo
                                     select pe).FirstOrDefaultAsync();
                if(dis != null)
                {
                    return BadRequest("El Horario con este codigo ya existe");
                }
            Horario hora = new Horario()
            {
              CodigoHorario = codigo,
              HoraInicio=HorarioInicio,
              HoraFin=HoraFin
            };
            await context.Horario.AddAsync(hora);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El Horario fue registrado correctamente"});
        }
        [HttpDelete("EliminarHorario")]
        public async Task<IActionResult> DeleteHorario(string codigo)
        {
            
            var xd = await (from ar in context.Horario
                             where ar.CodigoHorario == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("El Horario fue eliminado correctamente");
        }

    }
}