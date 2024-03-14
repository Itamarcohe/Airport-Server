using System.Security.Cryptography.X509Certificates;

namespace AirportServer.Models
{
    public class LegsJoinTable
    {
        public int Id { get; set; }
        public virtual Leg? FromLeg { get; set; }
        public virtual Leg? ToLeg { get; set; }
    }
}
