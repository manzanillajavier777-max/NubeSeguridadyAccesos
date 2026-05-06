using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Mapeador;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatosBiometricosController : ControllerBase
    {
        private readonly AppDbContext context;
        public DatosBiometricosController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListadePersonasResgistradasDTo")]
        public async Task<IActionResult> GetIdentidadDTO()
        {
            var dato = await context.DatosBiometricos.ToListAsync();
            var datos = dato.Select(i => i.toDatosBiometricopsDTO()).ToList();
             return Ok(datos);
            
        }
        [HttpGet("QueryListarDatoBiometricoDeCadaPersona")]
        public async Task<IActionResult> GetListarDatos(string codigo)
        {
            var query = await(from ide in context.IdentidadAcceso
                              join dato in context.DatosBiometricos on ide.Id_Identidad equals dato.IdentidadId
                              where ide.CodigoPersona == codigo
                              select new
                              {
                                  CodigoIdentidad = ide.CodigoPersona,
                                  TipoPersona = ide.TipoPersona,
                                  CodigoPersona = ide.CodigoPersona,
                                  TipoDeHuella = dato.TipoDatoBiometrico,
                                  FechaRegistro = dato.FechaRegistro,
                              }).ToListAsync();
            return Ok(query);
        } 
        [HttpGet("LIstadePersonaActivasDTO")]
        public async Task<IActionResult> GetDatosBioActivos()
        {
             var dato = await(from ar in context.DatosBiometricos
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var datos = dato.Select(i => i.toDatosBiometricopsDTO()).ToList();
            
            return Ok(datos);
            
        } 
        [HttpPut("ActuliazarDatoBio")]
        public async Task<IActionResult> PutDatosBio(string codigo, string TipoHuella,string TipoDato,DateOnly fechaRegistro)
        {
            var datos = await (from ar in context.DatosBiometricos
                                  where ar.CodigoDato == codigo
                                  select ar).FirstOrDefaultAsync();
            if (datos == null)
            {
                return NotFound("El dato Biometrico no fue encontrado");
            }
            datos.TipoDatoBiometrico = TipoDato;
            datos.DatoHuella = TipoHuella;
            datos.FechaRegistro=fechaRegistro;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "EL Dato Biometrico fue actualizado correctamente" });
        }

        [HttpPost("AnadirDatoBio")]
        public async Task<IActionResult> PostDatos(string coidgoidentidad,string codigo,string TipoDato,string DatoHuella,DateOnly fecharegistro )        
        {
            DatosBiometricos datos = await (from pe in context.DatosBiometricos
                                     where pe.CodigoDato == codigo
                                     select pe).FirstOrDefaultAsync();
                if(datos != null)
                {
                    return BadRequest("El Dato Biometrico con este codigo ya existe");
                }
            var ide = await(from id in context.IdentidadAcceso
                            where id.CodigoPersona==coidgoidentidad
                            select id).FirstOrDefaultAsync();
            if (ide == null)
            {
                return BadRequest("La persona no existe");
            }
            DatosBiometricos dato = new DatosBiometricos()
            {
              CodigoDato = codigo,
              TipoDatoBiometrico=TipoDato,
              DatoHuella=DatoHuella,
              FechaRegistro=fecharegistro,
              IdentidadId = ide.Id_Identidad,
              identiidad = ide
              
            };
            await context.DatosBiometricos.AddAsync(dato);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El Dato Biometrico fue registrado correctamente"});
        }
        [HttpDelete("EliminarDatoBiometrico")]
        public async Task<IActionResult> DeleteDatoBio(string codigo)
        {
            
            var xd = await (from ar in context.DatosBiometricos
                             where ar.CodigoDato == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("El dato Biometrico fue eliminado correctamente");
        }

    }
}