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
        public virtual DbSet<LegsJoinTable> LegsJoinTable { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        public void SeedData()
        {
            try
            {
                var leg1 = new Leg { Number = 1, CrossingTime = 3000/*, Status = LegStatusType.Arrival*/ };
                var leg2 = new Leg { Number = 2, CrossingTime = 2000 };
                var leg3 = new Leg { Number = 3, CrossingTime = 3000 };
                var leg4 = new Leg { Number = 4, CrossingTime = 2000
                   /* , Status = LegStatusType.Arrival | LegStatusType.Departure | LegStatusType.Exit*/ };
                var leg5 = new Leg { Number = 5, CrossingTime = 2500 };
                var leg6 = new Leg { Number = 6, CrossingTime = 1500 };
                var leg7 = new Leg { Number = 7, CrossingTime = 1000 };
                var leg8 = new Leg { Number = 8, CrossingTime = 2000
                    /*, Status = LegStatusType.Departure*/ };
                var leg9 = new Leg { Number = 9 };

                var FromLeg1 = new LegsJoinTable { ToLeg = leg2, FromLeg = leg1};
                var FromLeg2 = new LegsJoinTable { ToLeg = leg3, FromLeg = leg2 };
                var FromLeg3 = new LegsJoinTable { ToLeg = leg4 , FromLeg = leg3 };

                // Leg4 options
                var FromLeg4 = new LegsJoinTable { ToLeg = leg5 , FromLeg = leg4 };

                var FromLeg4To9 = new LegsJoinTable { ToLeg = leg9 , FromLeg = leg4 };

                // Leg5
                var FromLeg5To6 = new LegsJoinTable { ToLeg = leg6, FromLeg = leg5 };
                var FromLeg5To7 = new LegsJoinTable { ToLeg = leg7, FromLeg = leg5 };

                // leg 6
                var FromLeg6To8 = new LegsJoinTable { ToLeg = leg8, FromLeg = leg6 };

                // leg 7
                var FromLeg7To8 = new LegsJoinTable { ToLeg = leg8, FromLeg = leg7 };

                // leg8

                var FromLeg8To4 = new LegsJoinTable { ToLeg = leg4, FromLeg = leg8 };


                LegsJoinTable.AddRange(new List<LegsJoinTable> { FromLeg1 , FromLeg2, FromLeg3, FromLeg4, FromLeg4To9,
                FromLeg5To6, FromLeg5To7, FromLeg6To8, FromLeg7To8, FromLeg8To4});
                Legs.AddRange(new List<Leg> { leg1, leg2, leg3, leg4, leg5, leg6, leg7, leg8, leg9 });


                SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
