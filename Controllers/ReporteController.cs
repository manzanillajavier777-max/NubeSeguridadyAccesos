using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using DTOS;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly AppDbContext context;
        public ReporteController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("QueryListarReportesGeneradosPorCadaPersona")]
        public async Task<IActionResult> GetListarReportesGeneradosPorCadaPersona(string codigoidentidad)
        {
            var query = await (from ide in context.IdentidadAcceso
                               join rep in context.Reporte on ide.Id_Identidad equals rep.IdentidadId
                               where ide.CodigoPersona ==codigoidentidad
                               select new
                               {
                                   CodigoPersona = ide.CodigoPersona,
                                   TipoDeReporte = rep.TipoReporte,
                                   FechaReporte = rep.FechaReporte,
                                   Descripcion = rep.Descripcion
                               }).ToListAsync();
            if(query == null)
            {
                return BadRequest("Este Usuario no realizo ningun reporte");
            }
            return Ok(query);
        }
        [HttpGet("QueryListarReportesSegunSuTipo")]
        public async Task<IActionResult> GetListarReportesSegunSuTipo(string TipoReporte)
        {
            var reporte = await(from ide in context.IdentidadAcceso
                            join rep in context.Reporte on ide.Id_Identidad equals rep.IdentidadId
                            where rep.TipoReporte == TipoReporte
                            group rep by rep.TipoReporte into repo
                            select new
                            {
                                TipoReporte = repo.Key,
                                TotalDeReportes = repo.Count(),
                                Personas = repo.Select(p => new
                                {
                                    p.identidadd.CodigoPersona,
                                    p.codigoReporte,
                                    p.Descripcion,
                                    p.FechaReporte
                                }).ToList()
                            }).ToListAsync();
            return Ok(reporte);
        }
        [HttpGet("ListadeReportesDTO")]
        public async Task<IActionResult> GetReportesDTO()
        {
            var repo = await context.Reporte.ToListAsync();
            var reporte = repo.Select(r=>r.toReporteDTO()).ToList();
            return Ok(reporte);
            
        } 
        [HttpGet("LIstadeReportesActivosDTO")]
        public async Task<IActionResult> GetReportesActivos()
        {
            var repo = await(from ar in context.Reporte
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var reporte = repo.Select(r=>r.toReporteDTO()).ToList();
            return Ok(reporte);
            
        } 
        [HttpPut("ActuliazarReporte")]
        public async Task<IActionResult> PutReportes(string codigo,string tipo,string descripcion)
        {
            var datos = await (from ar in context.Reporte
                                  where ar.codigoReporte == codigo
                                  select ar).FirstOrDefaultAsync();
            if (datos == null)
            {
                return NotFound("El Reporte no fue encontrado");
            }
           datos.codigoReporte=codigo;
           datos.TipoReporte=tipo;
           datos.Descripcion=descripcion;
           datos.FechaReporte=DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "EL Reporte fue actualizado correctamente" });
        }

        [HttpPost("AnadirReporte")]
        public async Task<IActionResult> PostReportes(string codigoIedntidad,string codigoReporte,string tipo,string descripcion)        
        {
            Reporte rep = await (from pe in context.Reporte
                                     where pe.codigoReporte == codigoReporte
                                     select pe).FirstOrDefaultAsync();
                if(rep != null)
                {
                    return BadRequest("El Reporte con este codigo ya existe");
                }
            var ide = await(from id in context.IdentidadAcceso
                            where id.CodigoPersona==codigoIedntidad
                            select id).FirstOrDefaultAsync();
            if (ide == null)
            {
                return BadRequest("La persona no existe");
            }

            Reporte repo = new Reporte()
            {
              codigoReporte= codigoReporte,
              TipoReporte = tipo,
              Descripcion = descripcion,
              FechaReporte = DateTime.UtcNow,
              IdentidadId = ide.Id_Identidad,
              identidadd = ide
              
            };
            await context.Reporte.AddAsync(repo);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El Reporte fue registrado correctamente"});
        }
        [HttpDelete("EliminarReporte")]
        public async Task<IActionResult> DeleteReporte(string codigo)
        {
            
            var xd = await (from ar in context.Reporte
                             where ar.codigoReporte == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("El Reporte fue eliminado correctamente");
        }

    }
}