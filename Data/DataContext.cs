using AirportServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Leg> Legs { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Log> Logs { get; set; }


        public void SeedData()
        {
            try
            {
                // leg1
                var l1 = new Leg { Number = 1, CrossingTime = 1000, NextLegs = new List<int> { 2 }, Legs = new List<Leg>() };
                var l2 = new Leg { Number = 2, CrossingTime = 2000, NextLegs = new List<int> { 3 } };
                var l3 = new Leg { Number = 3, CrossingTime = 3000, NextLegs = new List<int> { 4 } };
                var l4 = new Leg { Number = 4, CrossingTime = 5000, NextLegs = new List<int> { 5, 9 }, Legs = new List<Leg>() };
                var l5 = new Leg { Number = 5, CrossingTime = 1000, NextLegs = new List<int> { 6, 7 } };
                var l6 = new Leg { Number = 6, CrossingTime = 2000, NextLegs = new List<int> { 8 }, Legs = new List<Leg>() };
                var l7 = new Leg { Number = 7, CrossingTime = 3000, NextLegs = new List<int> { 8 } , Legs = new List<Leg>() };
                var l8 = new Leg { Number = 8, CrossingTime = 5000, NextLegs = new List<int> { 4 } };
                var l9 = new Leg { Number = 9, CrossingTime = 5000 };




                l6.Legs.Add(l8);
                l7.Legs.Add(l8);

                l1.Legs.Add(l2);

                l4.Legs.Add(l5);
                l4.Legs.Add(l9);




                Legs.AddRange(new List<Leg> { l1, l2, l3, l4, l5, l6, l7, l8, l9 });

                SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
