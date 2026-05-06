using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispositivoController : ControllerBase
    {
        private readonly AppDbContext context;
        public DispositivoController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListadeDispositivosRegistradosDTO")]
        public async Task<IActionResult> GetDispositivo()
        {
             var dis = await context.Dispositivo.ToListAsync();
             var dispositivo = dis.Select(d => d.toDispositivoDTO()).ToList();
             return Ok(dispositivo);
            
        } 
        [HttpGet("LIstadeDispositivosActivasDTO")]
        public async Task<IActionResult> GetDispositivoActivos()
        {
             var dis = await(from ar in context.Dispositivo
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var dispositivo = dis.Select(d => d.toDispositivoDTO()).ToList();
             return Ok(dispositivo);
            
        } 
        [HttpGet("MostrarSegunElCodigoDTO")]
        public async Task<IActionResult> GetDispositivoCodigo(string codigo)
        {
            var dis = await(from ar in context.Dispositivo
                           where ar.NumeroSerie == codigo
                           select ar).ToListAsync();
            var dispositivo = dis.Select(d => d.toDispositivoDTO()).ToList();
             return Ok(dispositivo);
        }
        [HttpPut("ActuliazarDispositivo")]
        public async Task<IActionResult> PutDatosBio(string codigo, string nombre,string CodigoArea)
        {
            var datos = await (from ar in context.Dispositivo
                                  where ar.NumeroSerie == codigo
                                  select ar).FirstOrDefaultAsync();
            if (datos == null)
            {
                return NotFound("El Dispositivo no fue encontrado");
            }
            datos.NumeroSerie = codigo;
            datos.NombreDispositivo = nombre;
            datos.CodigoArea=CodigoArea;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "EL Dispositivo fue actualizado correctamente" });
        }

        [HttpPost("AnadirDispositivo")]
        public async Task<IActionResult> PostDatos(string codigo, string nombre,string CodigoArea )        
        {
            Dispositivo dis = await (from pe in context.Dispositivo
                                     where pe.NumeroSerie == codigo
                                     select pe).FirstOrDefaultAsync();
                if(dis != null)
                {
                    return BadRequest("El Dispositivo con este codigo ya existe");
                }
            Dispositivo dist = new Dispositivo()
            {
              NumeroSerie=codigo,
              NombreDispositivo=nombre,
              CodigoArea=CodigoArea
            };
            await context.Dispositivo.AddAsync(dist);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El Dispositivo fue registrado correctamente"});
        }
        [HttpDelete("EliminarDispositivo")]
        public async Task<IActionResult> DeleteDispositivo(string codigo)
        {
            
            var xd = await (from ar in context.Dispositivo
                             where ar.NumeroSerie == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("El Dispositivo fue eliminado correctamente");
        }

    }
}