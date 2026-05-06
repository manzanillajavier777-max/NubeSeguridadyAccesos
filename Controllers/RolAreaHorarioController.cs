using System.Net.Mime;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolAreaHorarioController : ControllerBase
    {
        private readonly AppDbContext context;
        public RolAreaHorarioController(AppDbContext context)
        {
            this.context = context; 
        }
        [HttpGet("QueryListarNumeroDeRoles")]
        public async Task<IActionResult>GetListarNumeroDeRoles()
        {
            var query = await(from rol in context.Rol
                              join rh in context.RolHorario on rol.Id_Rol  equals rh.rolId
                              select rol.Id_Rol
                              ).Distinct().CountAsync();
            return Ok(query);
        }
        [HttpGet("QueryMostarAreasQuePuedeAccederCadaRolIndividual")]
        public async Task<IActionResult> GetMostrarAreasQuePuedeAccederCadaRolIndividual(string nombreRol)
        {
            var query = await(from rol in context.Rol
                              join rh in context.RolHorario on rol.Id_Rol equals rh.rolId
                              join hor in context.Horario on rh.HoraioId equals hor.Id_Horario
                              where rol.Nombre == nombreRol
                              select new
                              {
                                  NombreRol = rol.Nombre,
                                  CodigoArea = rh.codigoArea,
                                  HoraInicio = hor.HoraInicio,
                                  HoraFin = hor.HoraFin
                              }).ToListAsync();
            return Ok(query);
        }
        [HttpGet("MostarAreasQuePuedeAccederCadaRol")]
        public async Task<IActionResult> GetMostrarAreasQuePuedeAccederCadaRol()
        {
            var query= await (from rol in context.Rol
                              join rah in context.RolHorario on rol.Id_Rol equals rah.rolId
                              join hor in context.Horario on rah.HoraioId equals hor.Id_Horario
                              select new
                              {
                                  NombreRol = rol.Nombre,
                                  HoraInicio = hor.HoraInicio,
                                  HoraFin = hor.HoraFin,
                                  Area = rah.codigoArea

                              }).ToListAsync();
            return Ok(query);
        }
        [HttpPost("AgregarHorario")]
        public async Task<IActionResult> PostAgregarHorario( string codigorol,string codigoArea,string codigoHorario)
        {
            Horario? hora = await(from horaa in context.Horario
                                 where horaa.CodigoHorario == codigoHorario
                                 select horaa).FirstOrDefaultAsync();
            Rol? rol = await (from role in context.Rol
                            where role.CodigoRol == codigorol
                            select role).FirstOrDefaultAsync();
            if(hora == null || rol == null)
            {
                return BadRequest("El horario o Rol no existe");
            }
            var rolHorario =await (from rh in context.RolHorario
                                    where rh.HoraioId == hora.Id_Horario && rh.rolId == rol.Id_Rol
                                    select rh).FirstOrDefaultAsync();
            if(rolHorario != null)
            {
                return BadRequest("La relacion ya existe");
            }
            var rolhora = new RolHorario
            {
                rolId = rol.Id_Rol,
                HoraioId =hora.Id_Horario,
                rol = rol,
                horario = hora,
                codigoArea =codigoArea
            };
            context.RolHorario.Add(rolhora);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="Horario Registrado Correctamente"});
        }
    }
}