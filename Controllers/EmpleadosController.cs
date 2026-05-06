using DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class EmpladosController : ControllerBase
    {
        private readonly HttpClient httpClient;
        public EmpladosController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        [HttpGet("ListarListaDeEmpleados")]
        public async Task<IActionResult> GetEmpleados()
        {
            var empleados = await  httpClient.GetFromJsonAsync<List<EmpleadosDto>>("");
            return Ok(empleados);
        }
    }
}