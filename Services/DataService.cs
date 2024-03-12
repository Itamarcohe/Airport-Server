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

        public DataService(IServiceScopeFactory scopeService)
        {
            data = scopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            LegsList = data.Legs.ToList();
        }
        public async void AddNewFlight(Flight flight)
        {
            if (!IsAirportFull() && !IsLegOccupated(1))
            {
                PrintToConsole(flight);
                flight.Leg = GetLeg(1);
                AddFlight(flight);
                AddLog(flight);
                await data.SaveChangesAsync();
                flight.CallbackCalled += GoNextTerminal;
            }
        }

        private async void GoNextTerminal(Flight flight)
        {
            flight.CallbackCalled -= GoNextTerminal;
            flight.LegTimer.Change(Timeout.Infinite, Timeout.Infinite);
            bool isNext = false;
            int modifiedNextLegNumber = -1;

            foreach (int nextLegNumber in flight.Leg.NextLegs!)
            {
                if (flight.Leg.Number == 4)
                {
                    if (flight.Status == StatusType.Arrival)
                    {
                        modifiedNextLegNumber = 5;
                    }

                    else if (flight.Status == StatusType.Departure)
                    {
                        flight.Status = StatusType.DoneDepartured;
                        flight.Leg = GetLeg(9);
                        await SaveAsync();
                        Console.WriteLine("Finished reached 9 watch the logs~!");
                        break;
                    }

                    if (!IsLegOccupated(modifiedNextLegNumber))
                    {
                        MoveLeg(flight, modifiedNextLegNumber);
                        isNext = true;
                        await SaveAsync();
                    }
                    else
                    {
                        GoNextTerminal(flight);
                    }
                    flight.CallbackCalled += GoNextTerminal;
                    break;
                }

                else
                {
                    if (flight.Leg.Number == 5)
                    {
                        flight.Status = StatusType.Departure;
                    }

                    if (!IsLegOccupated(nextLegNumber))
                    {
                        MoveLeg(flight, nextLegNumber);
                        isNext = true;
                        await SaveAsync();
                        flight.CallbackCalled += GoNextTerminal;
                        break;
                    }
                }
                if (!isNext)
                {
                    PrintToConsole(flight);
                    Console.WriteLine($"\n\n ---- Waiting another 20 SEC NOW ---- \n\n");
                    await WaitAsync(20000);
                    if (flight.Leg.NextLegs.Count == 1)
                    {
                        GoNextTerminal(flight);
                    }
                }
            }
        }

        private void MoveLeg(Flight flight, int nextLegNumber)
        {
            AddOutTimeToLog(flight);
            flight.Leg = GetLeg(nextLegNumber);
            AddLog(flight);
            PrintToConsole(flight);
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
            $"\n --Current Leg = {flight.Leg!.Number}\n --Gonna wait = {flight.Leg.CrossingTime * 5 / 1000} SEC \n CurrentTime {DateTime.Now}\n\n\n");
        private async Task WaitAsync(int millisecondsToWait) => await WaitAsync(millisecondsToWait);
        private void AddLog(Flight flight) => data.Logs.Add(new Log { Flight = flight, Leg = flight.Leg, In = DateTime.Now });
        private void AddFlight(Flight flight) => data.Flights.Add(flight);
        private bool IsAirportFull() => data.Flights.Count(f => f.Leg != null && f.Leg.Number >= 1 && f.Leg.Number <= 8) >= 4;
        private bool IsLegOccupated(int number) => data.Flights.Any(f => f.Leg != null && f.Leg.Number == number);
        private Leg GetLeg(int number) => LegsList.Find(l => l.Number == number)!;
        private async Task SaveAsync() => await data.SaveChangesAsync();
    }
}
