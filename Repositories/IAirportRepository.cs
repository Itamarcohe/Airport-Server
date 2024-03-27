using AirportServer.Models;

namespace AirportServer.Repositories
{
    public interface IAirportRepository
    {
        Task AddFlightAsync(Flight flight); 
        Task<bool> IsAirportFullAsync(); 
        Task<bool> IsLegOccupiedAsync(Leg leg); 
        Task<List<Leg>> GetAllLegsAsync(); 
        Task<List<LegsJoinTable>> GetAllLegsJoinAsync(); 
        Task AddLogAsync(Log log);
        Task<Log> UpdateOutLogTime(int flightId); 
        Task SaveAsync(); 
    }
}


