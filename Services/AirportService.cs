using AirportServer.Data;
using AirportServer.Models;
using Microsoft.EntityFrameworkCore;
using AirportServer.Repositories;
using AirportServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace AirportServer.Services
{
    public class AirportService
    {
        private readonly IAirportRepository _airportRepository;
        private List<Leg> LegsList { get; set; } = [];
        private List<LegsJoinTable> LegsJoinList { get; set; } = [];

        private readonly IHubContext<AirportHub> _hubContext;
        private async Task SaveAsync() => await _airportRepository.SaveAsync();

        public AirportService(IAirportRepository airportRepository, IHubContext<AirportHub> hubContext)
        {
            _airportRepository = airportRepository;
            _hubContext = hubContext;
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            LegsList = await _airportRepository.GetAllLegsAsync();
            LegsJoinList = await _airportRepository.GetAllLegsJoinAsync();
        }

        public async Task AddNewFlight(Flight flight)
        {
            Leg leg1 = LegsList.Find(l => l.Number == 1)!;

            if (!await _airportRepository.IsAirportFullAsync() && !await IsLegOccupiedAsync(leg1))
            {
                flight.Leg = leg1;
                await _airportRepository.AddFlightAsync(flight);
                await AddLogAsync(flight);
                await _airportRepository.SaveAsync();
                flight.CallbackCalled += NextLeg;
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
                    if (!await IsLegOccupiedAsync(nextLeg))
                    {
                        ChangeStatusToDeparture(flight);
                        await MoveLeg(flight, nextLeg);
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
                await BeforeDestinationArrival(flight, nextLegs);
            }
            else if (flight.Status == StatusType.Departure)
            {
                await ArrivingDestination(flight, nextLegs);
            }
        }

        private async Task BeforeDestinationArrival(Flight flight, List<LegsJoinTable> nextLegs)
        {
            Leg nextLeg5 = nextLegs.Find(nl => nl.ToLeg!.Number == 5)!.ToLeg!;
            if (!await IsLegOccupiedAsync(nextLeg5))
            {
                await MoveLeg(flight, nextLeg5);
            }
            else
            {
                await WaitAsync(2);
                await BeforeDestinationArrival(flight, nextLegs);
            }
        }

        private async Task ArrivingDestination(Flight flight, List<LegsJoinTable> nextLegs)
        {
            Leg nextLeg9 = nextLegs.Find(nl => nl.ToLeg!.Number == 9)!.ToLeg!;
            flight.Status = StatusType.DoneDepartured;
            await MoveLeg(flight, nextLeg9);
            await SaveAsync();
        }
        private async Task<bool> IsLegOccupiedAsync(Leg leg)
        {
            return await _airportRepository.IsLegOccupiedAsync(leg);
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

        private static async Task WaitAsync(int secondToWait)
        {
            await Task.Delay(secondToWait * 1000);
        }

        private async Task MoveLeg(Flight flight, Leg leg)
        {
            Log log = await _airportRepository.UpdateOutLogTime(flight.Id);
            await _hubContext.Clients.All.SendAsync("UpdateLogs", log);
            flight.Leg = leg;
            await AddLogAsync(flight);
            await SaveAsync();
            flight.CallbackCalled += NextLeg;
        }

        public async Task AddLogAsync(Flight flight)
        {
            Log log = new Log { Flight = flight, Leg = flight.Leg, In = DateTime.Now };
            await _airportRepository.AddLogAsync(log);
            await _hubContext.Clients.All.SendAsync("UpdateLogs", log);
        }

    }
}
