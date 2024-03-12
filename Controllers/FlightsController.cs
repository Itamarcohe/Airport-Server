using AirportServer.Data;
using AirportServer.Models;
using AirportServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirportServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DataContext data;
        private readonly DataService service;

        public FlightsController(DataContext data, DataService service)
        {
            this.data = data;
            this.service = service;
            data.Database.Migrate();
        }

        [HttpGet]
        public void SeedData()
        {
            data.SeedData();
        }


        [HttpPost]
        public void AddFlight(Flight flight)
        {
            service.AddNewFlight(flight);
        }

        private void AddLog(Flight flight)
        {
            data.Logs.Add(new Log { Flight = flight, Status = flight.Status, Leg = flight.Leg, In = DateTime.Now });
            data.SaveChanges();
        }
    }
}
