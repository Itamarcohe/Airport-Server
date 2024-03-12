namespace AirportServer.Models
{
    public class Log
    {
        public int Id { get; set; }
        public virtual Flight Flight { get; set; }
        public virtual Leg Leg { get; set; }
        public StatusType Status { get; set; }
        public DateTime In { get; set; }
        public DateTime? Out { get; set; }

    }
}
