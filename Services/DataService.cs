using AirportServer.Data;
using AirportServer.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportServer.Services
{
    public class DataService
    {
        private readonly DataContext data;
        private List<Leg> LegsList { get; set; }
        private List<LegsJoinTable> LegsJoinList { get; set; }

        public DataService(IServiceScopeFactory scopeService)
        {
            data = scopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

            LegsList = data.Legs.ToList();

            LegsJoinList = data.LegsJoinTable
                .Include(ljt => ljt.FromLeg)
                .Include(ljt => ljt.ToLeg)
                .ToList();
        }

        public async void AddNewFlight(Flight flight)
        {
            Leg leg1 = LegsList.Find(l => l.Number == 1)!;

            if (!IsAirportFull() && !IsLegOccupated(leg1))
            {
                flight.Leg = leg1;
                AddFlight(flight);
                AddLog(flight);
                await data.SaveChangesAsync();
                flight.CallbackCalled += NextLeg;
                PrintToConsole(flight);
            } 
        }

        private async void NextLeg(Flight flight)
        {
            flight.CallbackCalled -= NextLeg;
            flight.LegTimer.Change(Timeout.Infinite, Timeout.Infinite);
            List<LegsJoinTable> LegConnections = LegsJoinList.Where(ljt => ljt.FromLeg == flight.Leg).ToList();
            if (flight.Leg.Number == 4)
            {
                await ProccessLeg4(flight, LegConnections);
            }
            else
            {
                for (int i = 0; i < LegConnections.Count; i++)
                {
                    Leg nextLeg = LegConnections[i].ToLeg!;
                    if (!IsLegOccupated(nextLeg))
                    {
                        ChangeStatusToDeparture(flight);
                        MoveLeg(flight, nextLeg);
                        await SaveAsync();
                        flight.CallbackCalled += NextLeg;
                        break;
                    }
                    else
                    {
                        await NextLegOptionOrWait(flight, LegConnections, i);
                    }
                }
            }
        }
        private async Task ProccessLeg4(Flight flight, List<LegsJoinTable> nextLegs)
        {
            if (flight.Status == StatusType.Arrival)
            {
                Leg nextLeg5 = nextLegs.Find(nl => nl.ToLeg!.Number == 5)!.ToLeg!;
                if (!IsLegOccupated(nextLeg5))
                {
                    MoveLeg(flight, nextLeg5);
                    await SaveAsync();
                    flight.CallbackCalled += NextLeg;
                }
                else
                {
                    await WaitAsync(5);
                    flight.CallbackCalled += NextLeg;
                }
            }
            else if (flight.Status == StatusType.Departure)
            {
                Leg nextLeg9 = nextLegs.Find(nl => nl.ToLeg!.Number == 9)!.ToLeg!;
                flight.Status = StatusType.DoneDepartured;
                MoveLeg(flight, nextLeg9);
                await SaveAsync();
                Console.WriteLine("Finished reached 9 watch the logs~!");
            }
        }

        private async Task NextLegOptionOrWait(Flight flight, List<LegsJoinTable> nextLegs, int i)
        {
            if (nextLegs.Count == 1 || i == 1)
            {
                await WaitAsync(5);
                NextLeg(flight);
            }
        }

        private static void ChangeStatusToDeparture(Flight flight)
        {
            if (flight.Leg.Number == 5)
            {
                flight.Status = StatusType.Departure;
            }
        }
        private async Task WaitAsync(int secondToWait)
        {
            await Task.Delay(secondToWait * 1000);
        }
        private void MoveLeg(Flight flight, Leg leg)
        {
            AddOutTimeToLog(flight);
            flight.Leg = leg;
            AddLog(flight);
            PrintToConsole(flight);
        }
        private void AddOutTimeToLog(Flight flight)
        {
            var latestLog = data.Logs
                .Include(l => l.Flight)
                .Where(l => l.Flight.Id == flight.Id)
                .OrderByDescending(l => l.Id)
                .FirstOrDefault();

            latestLog!.Out = DateTime.Now;
        }
        private static void PrintToConsole(Flight flight) => Console.WriteLine($"\n\n\n--Flight Code: {flight.Code}--" +
            $"\n --Current Leg = {flight.Leg!.Number}\n --Gonna wait = {flight.Leg.CrossingTime * 3 / 1000} SEC \n CurrentTime {DateTime.Now}\n\n\n");
        private void AddLog(Flight flight) => data.Logs.Add(new Log { Flight = flight, Leg = flight.Leg, In = DateTime.Now });
        private void AddFlight(Flight flight) => data.Flights.Add(flight);
        private bool IsAirportFull() => data.Flights.Count(f => f.Leg != null && f.Leg.Number >= 1 && f.Leg.Number <= 8) >= 4;
        private bool IsLegOccupated(Leg leg) => data.Flights.Any(f => f.Leg != null && f.Leg == leg);
        private Leg GetLeg(Leg leg) => LegsList.Find(l => l == leg)!;
        private async Task SaveAsync() => await data.SaveChangesAsync();
    }
}