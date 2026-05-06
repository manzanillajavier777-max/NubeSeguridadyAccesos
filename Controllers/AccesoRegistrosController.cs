using System.Security.AccessControl;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesosRegistrosController : ControllerBase
    {
        private readonly AppDbContext context;
        public AccesosRegistrosController(AppDbContext context)
        {
            this.context = context; 
        }

        [HttpGet("QueryListarAccesosExitosos")]
        public async Task<IActionResult> GetListarAccesoDePersonas()
        {
            var query= await (from dis in context.Dispositivo
                              join acc in context.AccesoRegistros on dis.Id_Dispositivo equals acc.DispositivoId
                              join ide in context.IdentidadAcceso on acc.IdentidadId equals ide.Id_Identidad
                              where acc.Descripcion == "Acceso Exitoso"
                              select new
                              {
                                  Descripcion = acc.Descripcion,
                                  Identidad = ide.TipoPersona,
                                  CodigoPersona = ide.CodigoPersona,
                                  HoraRegistro = acc.HorarioRegistro,
                                  Area = dis.CodigoArea,
                                  NombreDispositivo = dis.NombreDispositivo
                                  

                              }).ToListAsync();
            return Ok(query);
        }
        [HttpGet("QueryListarAccesosDeCadaPersonaIndividual")]
        public async Task<IActionResult> GetListarAccesosDeCadaPersona(string codigodientidad)
        {   
            var query = await(from ide in context.IdentidadAcceso
                              join ar in context.AccesoRegistros on ide.Id_Identidad equals ar.IdentidadId
                              join dis in context.Dispositivo on ar.DispositivoId equals dis.Id_Dispositivo
                              where ide.CodigoPersona == codigodientidad
                              select new
                              {
                                  CodigoPersona = ide.CodigoPersona,
                                  HoraREgistro = ar.HorarioRegistro,
                                  Dispositivo = dis.NombreDispositivo,
                                  Area = ar.CodigoArea,
                                  Descripcion = ar.Descripcion
                              }).ToListAsync();
            if(query == null)
            {
                return BadRequest("El codigo de la persona no fue encontrada");
            }
            return Ok(query);
            
        }

        [HttpGet("QueryListarAccesosDeCadaArea")]
        public async Task<IActionResult> GetListarAccesosDeCadaArea(string codigoarea)
        {   
            var query = await(from ide in context.IdentidadAcceso
                              join ar in context.AccesoRegistros on ide.Id_Identidad equals ar.IdentidadId
                              join dis in context.Dispositivo on ar.DispositivoId equals dis.Id_Dispositivo
                              where ar.CodigoArea == codigoarea
                              select new
                              {
                                  CodigoPersona = ide.CodigoPersona,
                                  HoraREgistro = ar.HorarioRegistro,
                                  Dispositivo = dis.NombreDispositivo,
                                  Area = ar.CodigoArea,
                                  Descripcion = ar.Descripcion
                              }).ToListAsync();
            if(query == null)
            {
                return BadRequest("El codigo del area no fue encontrada");
            }
            return Ok(query);
            
        }

        [HttpGet("QueryListarAccesosDenegados")]
        public async Task<IActionResult> GetListarAccesosDenegados()
        {
            var query= await (from dis in context.Dispositivo
                              join acc in context.AccesoRegistros on dis.Id_Dispositivo equals acc.DispositivoId
                              join ide in context.IdentidadAcceso on acc.IdentidadId equals ide.Id_Identidad
                              where acc.Descripcion == "Acceso Denegado"
                              select new
                              {
                                  Acceso = acc.Descripcion,
                                  Identidad = ide.TipoPersona,
                                  CodigoPersona = ide.CodigoPersona,
                                  HoraRegistro = acc.HorarioRegistro,
                                  Area = dis.CodigoArea,
                                  NombreDispositivo = dis.NombreDispositivo

                              }).ToListAsync();
            return Ok(query);
        }

        [HttpPost("RegistrarAcceso")]
        public async Task<IActionResult> PostRegistros(string codigoDispositivo, string codigoidentidad, string codigoArea)
        {
            Dispositivo? dis = await (from acc in context.Dispositivo
                                              where acc.NumeroSerie == codigoDispositivo
                                              select acc).FirstOrDefaultAsync();
                                              
            IdentidadAcceso? identidad = await (from id in context.IdentidadAcceso
                                              where id.CodigoPersona == codigoidentidad
                                              select id).FirstOrDefaultAsync();
                                    
            if(identidad == null || dis == null)
            {

                return BadRequest("Dispositivo o identidad no encontrado");
            }

            var verificarAcceso = await(from rolp in context.RolIdentidad
                                        join rolArea in context.RolHorario on rolp.RolId equals rolArea.rolId
                                        where codigoArea == rolArea.codigoArea && identidad.Id_Identidad == rolp.IdentidadId
                                        select rolp).FirstOrDefaultAsync();
            if(verificarAcceso == null)
            {
                AccesoRegistros Denegado = new AccesoRegistros()
                {
                    IdentidadId = identidad.Id_Identidad,
                    DispositivoId = dis.Id_Dispositivo,
                    identidad =identidad,
                    dispositivo=dis,
                    FechaRegistro = new DateOnly(2025, 10, 10),
                    HorarioRegistro = DateTime.UtcNow,
                    Descripcion = "Acceso Denegado",
                    CodigoArea=codigoArea
                };
                
                context.AccesoRegistros.Add(Denegado);
                await context.SaveChangesAsync();
                return BadRequest("Acceso Denegado");
            }

            AccesoRegistros?  accesos = await(from acc in context.AccesoRegistros
                                              where acc.IdentidadId == identidad.Id_Identidad && acc.DispositivoId == dis.Id_Dispositivo
                                              select acc).FirstOrDefaultAsync();
            if(accesos!= null)
            {
                return BadRequest("La asignacion ya existe");
            }
        
            AccesoRegistros acces = new AccesoRegistros()
            {
                IdentidadId = identidad.Id_Identidad,
                DispositivoId = dis.Id_Dispositivo,
                identidad =identidad,
                dispositivo=dis,
                FechaRegistro = new DateOnly(2025, 10, 10),
                HorarioRegistro = DateTime.UtcNow,
                Descripcion="Acceso Exitoso",
                CodigoArea=codigoArea

            };
            
            context.AccesoRegistros.Add(acces);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="EL Acceso fue registrado correctamente"});
        }
    }
}