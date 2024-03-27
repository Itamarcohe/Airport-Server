using AirportServer.Data;
using AirportServer.Models;
using AirportServer.Repositories;
using AirportServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirportServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AirportService _service;

        public FlightsController(AirportService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task AddFlight(Flight flight)
        {
            await _service.AddNewFlight(flight);
        }

    }
}
