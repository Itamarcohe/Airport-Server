using AirportServer.Data;
using AirportServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

namespace AirportServer.Repositories
{
    public class AirportRepository : IAirportRepository
    {

        private readonly DataContext _data;

        public AirportRepository(IServiceScopeFactory scopeService)
        {
            _data = scopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
        }

        public async Task AddFlightAsync(Flight flight)
        {
            _data.Flights.Add(flight);
            await _data.SaveChangesAsync();
        }

        public async Task AddLogAsync(Log log)
        {
            _data.Logs.Add(log);
            await _data.SaveChangesAsync();
        }

        public async Task<List<Leg>> GetAllLegsAsync()
        {
            return await _data.Legs.ToListAsync();

        }

        public async Task<List<LegsJoinTable>> GetAllLegsJoinAsync()
        {
            return await _data.LegsJoinTable
                .Include(ljt => ljt.FromLeg)
                .Include(ljt => ljt.ToLeg)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogs()
        {
            return await _data.Logs.Include(l => l.Flight).ToListAsync();
        }


        public async Task<Log> UpdateOutLogTime(int flightId)
        {
            var latestLog = await _data.Logs
            .Include(l => l.Flight)
            .Where(l => l.Flight.Id == flightId)
            .OrderByDescending(l => l.Id)
            .FirstOrDefaultAsync();

            if (latestLog != null)
            {
                latestLog.Out = DateTime.Now;
                await _data.SaveChangesAsync();
            }

            return latestLog!;
        }


        public async Task<bool> IsAirportFullAsync()
        {
            var flightsCount = await _data.Flights
                .CountAsync(f => f.Leg != null && f.Leg.Number >= 1 && f.Leg.Number <= 8);
            return flightsCount >= 4;

        }

        public async Task<bool> IsLegOccupiedAsync(Leg leg)
        {
            return await _data.Flights
                .Where(f => f.Leg != null && f.Leg.Number >= 1 && f.Leg.Number <= 8)
                .AnyAsync(f => f.Leg == leg);
        }

        public async Task SaveAsync()
        {
            await _data.SaveChangesAsync();
        }
    }
}