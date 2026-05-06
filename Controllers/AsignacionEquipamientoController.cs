using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionEquipamientoController : ControllerBase
    {
        private readonly AppDbContext context;
        public AsignacionEquipamientoController(AppDbContext context)
        {
            this.context = context; 
        }

        [HttpGet("QueryListarTodosLasAsignacioes")]
        public async Task<IActionResult> GetAsignacionesCompletas()
        {
            var query= await (from equi in context.Equipamiento
                              join asig in context.AsignacionEquipamientos on equi.Id_Equipamiento equals asig.equipamientoId
                              join ide in context.IdentidadAcceso on asig.identidadId equals ide.Id_Identidad
                              where asig.Estado == "Activo" || asig.Estado == "Inactivo"
                              select new
                              {
                                  Equipamiento = equi.Nombre_Equipo,
                                  AsignacionEquipamiento = asig.fechaEntrega,
                                  IdentidadAcceso = ide.TipoPersona,
                                  CodigoIdentidad = ide.CodigoPersona
                              }).ToListAsync();
            return Ok(query);
        }
        [HttpGet("QueryContarEquioamientoAsignadoACadaPersona")]
        public async Task<IActionResult> GetContarEquioamientoAsignadoACadaPersona(string codigoidentidad)
        {
            var query = await(from ide in context.IdentidadAcceso
                              join ae in context.AsignacionEquipamientos on ide.Id_Identidad equals ae.identidadId
                              join equi in context.Equipamiento on ae.equipamientoId equals equi.Id_Equipamiento
                              where ide.CodigoPersona == codigoidentidad
                              group equi by new
                              {
                                  ide.Id_Identidad,
                                  ide.CodigoPersona
                              }into objetos
                              select new
                              {
                                  objetos.Key.CodigoPersona,
                                  TotalEquipos = objetos.Count(),
                                  Equipos = objetos.Select(e => new
                                  {
                                      e.Nombre_Equipo,
                                      e.Descripcion
                                  }).ToList()
                              }).ToListAsync();
            var pe = await(from ide in context.IdentidadAcceso
                            where ide.CodigoPersona == codigoidentidad
                            select ide).FirstOrDefaultAsync();
            if (pe == null)
            {
                return BadRequest("No se encontro a la persona ingresada");
            }
            return Ok(query);
        }


        [HttpGet("QueryVerEquipamientoAsignadoAUnaPersona")]
        public async Task<IActionResult> GetVerEquipamientoAsignadoAUnaPersona( string nombreEquipo)
        {
            var query= await (from asig in context.AsignacionEquipamientos
                              join equi in context.Equipamiento on asig.equipamientoId equals equi.Id_Equipamiento
                              where equi.Nombre_Equipo == nombreEquipo && asig.Estado == "Activo"
                              select new
                              {
                                  Equipamiento = equi.Nombre_Equipo,
                                  AsignacionEquipamiento = asig.fechaEntrega,
                                  IdentidadAcceso = asig.identidad.TipoPersona,
                                  CodigoPersona = asig.identidad.CodigoPersona
                              }).ToListAsync();
            if(query == null)
            {
                return NotFound($"El objeto{nombreEquipo} no fue asignado a ninguna persona");
            }
            return Ok(query);
        }

        
        [HttpPost("AnadirAsignacion")]
        public async Task<IActionResult> PostAsignacion(string codigoEqupamiento, string codigoidentidad, DateOnly fechaEntrega)
        {
            IdentidadAcceso? identidad = await (from id in context.IdentidadAcceso
                                              where id.CodigoPersona == codigoidentidad
                                              select id).FirstOrDefaultAsync();
                                    
            Equipamiento? equipo = await(from equi in context.Equipamiento
                                        where equi.CodigoEquipamiento==codigoEqupamiento
                                        select equi).FirstOrDefaultAsync();
            if(identidad == null || equipo == null)
            {
                return BadRequest("equipamiento o identidad no encontrado");
            }

            AsignacionEquipamiento? asiganacion = await(from asig in context.AsignacionEquipamientos
                                                       where asig.identidadId == identidad.Id_Identidad && asig.equipamientoId == equipo.Id_Equipamiento
                                                       select asig).FirstOrDefaultAsync();
            if(asiganacion != null)
            {
                return BadRequest("La asignacion ya existe");
            }
            AsignacionEquipamiento asignar = new AsignacionEquipamiento()
            {
                identidadId = identidad.Id_Identidad,
                equipamientoId = equipo.Id_Equipamiento,
                fechaEntrega = fechaEntrega,
                identidad = identidad,
                equipamiento = equipo
            };
            context.AsignacionEquipamientos.Add(asignar);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="La asignacion de equipo fue registrada correctamente"});
        }
        [HttpDelete("EliminarAsignacion")]
        public async Task<IActionResult>DeleteAsignacion(string codigoEquipamiento, string codigoIdentidad)
        {
            AsignacionEquipamiento? asignar = await (from asig in context.AsignacionEquipamientos
                                                where asig.equipamiento.CodigoEquipamiento == codigoEquipamiento && asig.identidad.CodigoPersona == codigoIdentidad
                                                select asig)
                                                .Include(asig => asig.equipamiento)
                                                .Include(asig => asig.identidad)
                                                .FirstOrDefaultAsync();
            if(asignar == null)
            {
                return NotFound("Nose encontro la agsignacion con estos roles");
            }
            asignar.Estado= "Inactivo";
            context.AsignacionEquipamientos.Update(asignar);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="La asignacion fue eliminada correctamente"});
        }
    }
}