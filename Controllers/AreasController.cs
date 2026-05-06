using DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class AreaController : ControllerBase
    {
        private readonly HttpClient httpClient;
        public AreaController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        [HttpGet("ListarAreasDelHospital")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await  httpClient.GetFromJsonAsync<List<AreasDTO>>("");
            return Ok(areas);
        }
    }
}