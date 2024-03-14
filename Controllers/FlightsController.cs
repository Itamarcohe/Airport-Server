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

            //Delete the existing database if it exists
            //data.Database.EnsureDeleted();

            //Create a new database
            //data.Database.EnsureCreated();

        }

        //[HttpGet]
        //public void SeedData()
        //{
        //    data.SeedData();
        //}
        [HttpGet]
        public IActionResult TestLegs()
        {
            var nextLegsForLegNumber4 = data.LegsJoinTable
                .Include(jointable => jointable.FromLeg)
                .Include(jointable => jointable.ToLeg)
                .Where(jointable => jointable.FromLeg.Id == 4)
                .ToList();

            return Ok(nextLegsForLegNumber4);
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
