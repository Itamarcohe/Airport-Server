namespace AirportServer.Models
{
    public class Leg
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int CrossingTime { get; set; }
        public List<int>? NextLegs { get; set; }
        public List<Leg> Legs {  get; set; }

    }

    // 
}
