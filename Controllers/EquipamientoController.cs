using Data;
using Entities;
using Mapeador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipamientoController : ControllerBase
    {
        private readonly AppDbContext context;
        public EquipamientoController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("MostrarEquipamientosCompletosDTO")]
        public async Task<IActionResult> GetEquipamientos()
        {
            var equi = await context.Equipamiento.ToListAsync();
            var equipamiento = equi.Select(e => e.toEquipamientoDTO()).ToList();
            return Ok(equipamiento);
            
        } 
        [HttpGet("MostrarEquipamientosActivosDTO")]
        public async Task<IActionResult> GetEquipamientoActivos()
        {
            var equi = await(from ar in context.Equipamiento
                           where ar.Estado == "Activo"
                           select ar).ToListAsync();
            var equipamiento = equi.Select(e => e.toEquipamientoDTO()).ToList();
            return Ok(equipamiento);
        }
        [HttpGet("MostrarEquipamientosPorElCodigoDTO")]
        public async Task<IActionResult> GetEquipamientoPorCodigo(string codigo)
        {
            var equi = await(from ar in context.Equipamiento
                           where ar.CodigoEquipamiento == codigo
                           select ar).ToListAsync();
            var equipamiento = equi.Select(e => e.toEquipamientoDTO()).ToList();
            return Ok(equipamiento);
        }

        [HttpPost("AnadirEquipamiento")]
        public async Task<IActionResult> PostEquipamiento(string nombre, string descripcion, string codigo)
        {
            Equipamiento equipamiento = await (from eq in context.Equipamiento
                                     where eq.CodigoEquipamiento == codigo
                                     select eq).FirstOrDefaultAsync();
                if(equipamiento != null)
                {
                    return BadRequest("El equipamiento con este codigo ya existe");
                }
            Equipamiento equi = new Equipamiento()
            {
                Nombre_Equipo= nombre,
                Descripcion=descripcion,
                CodigoEquipamiento=codigo
            };
            await context.Equipamiento.AddAsync(equi);
            await context.SaveChangesAsync();
            return Ok(new{mensaje="El equipamiento fue registrado correctamente"});
        }

        [HttpPut("ActualizarEquipamiento")]
        public async Task<IActionResult> PutEquipamiento(string codigo, string nombre, string descripcion)
        {
            Equipamiento equipamiento = await (from ar in context.Equipamiento
                                  where ar.CodigoEquipamiento == codigo
                                  select ar).FirstOrDefaultAsync();
            if (equipamiento == null)
            {
                return NotFound("El equipamiento no fue encontrado");
            }
            equipamiento.Nombre_Equipo = nombre;
            equipamiento.Descripcion = descripcion;
            equipamiento.CodigoEquipamiento = codigo;
            await context.SaveChangesAsync();
            return Ok(new { mensaje = "El equipamiento fue actualizado correctamente" });
        }

        
        [HttpDelete("EliminarEquipamiento")]
        public async Task<IActionResult> DeleteEquipamiento(string codigo)
        {
            
            var xd = await (from ar in context.Equipamiento
                             where ar.CodigoEquipamiento == codigo
                             select ar).FirstAsync();
            xd.Estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("Equipamiento Eliminado correctamente");
        }

    }
}