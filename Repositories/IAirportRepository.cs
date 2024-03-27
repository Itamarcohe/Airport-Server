using AirportServer.Models;

namespace AirportServer.Repositories
{
    public interface IAirportRepository
    {
        Task AddFlightAsync(Flight flight); // Flight Repository 
        Task<bool> IsAirportFullAsync(); //  Flight Repository
        Task<bool> IsLegOccupiedAsync(Leg leg); //  Airport Service ? //Airpost
        Task<List<Leg>> GetAllLegsAsync(); // Legs 
        Task<List<LegsJoinTable>> GetAllLegsJoinAsync(); // Legs
        Task AddLogAsync(Log log); // Log
        Task<Log> UpdateOutLogTime(int flightId); // Airport service but get the logs from Log Repo
        Task SaveAsync(); //
    }
}


